#!/bin/bash

READY_CHECK_COUNT=10
READY_CHECK_TIMEOUT=30

set - e

cd /root/image-edit-deployment

k='/snap/bin/microk8s.kubectl'
h='/snap/bin/helm'


$k get ns | grep image-edit

if [ $? -ne 0 ]; then
    echo "Creating image-edit namespace"
    $k create ns image-edit
fi

$h upgrade image-edit ./helm-chart -i


function checkReady () {
    echo "Checking if $2 is ready"

    STATUS=`echo "$1" | tr -s " " | cut -f3 -d " "`
    COUNT=`echo "$1" | tr -s " " | cut -f2 -d " "`
    CURRENT=`echo "$COUNT" | cut -f1 -d "/"`
    EXPECTED=`echo "$COUNT" | cut -f2 -d "/"`
    if [ "$CURRENT" == "$EXPECTED" ]; then
        if [ "$STATUS" != "Running" ]; then
            echo "$2 is in invalid status $STATUS"
        fi
        echo "$CURRENT of $EXPECTED instances of $2 are running"
        return 0
    fi
    echo "$CURRENT of $EXPECTED instances of $2 are running"
    return 1
}


i=0

while [ "$i" -ne "$READY_CHECK_COUNT" ]; do

    #TODO proper pod names
    IMAGE_EDIT_WEB=`$k get pods -n image-edit | sed "2q;d"`
    IMAGE_EDIT_DB=`$k get pods -n image-edit | sed "3q;d"`
    IMAGE_EDIT_RABBITMQ=`$k get pods -n image-edit | sed "4q;d"`

    checkReady "$IMAGE_EDIT_DB" "image-edit-db"
    r1=$?

    checkReady "$IMAGE_EDIT_RABBITMQ" "image-edit-rabbitmq"
    r2=$?

    checkReady "$IMAGE_EDIT_WEB" "image-edit-web"
    r3=$?

    if [ $((r1+r2+r3)) -eq 0 ]; then
        echo "Pods are ready"
        break
    fi

    echo "Sleeping"
    sleep "$READY_CHECK_TIMEOUT"s
    i=$((i+1))
    echo "Rertrying check..."
done

if [ "$i" -eq "$READY_CHECK_COUNT" ]; then
    echo "Reached max READY_CHECK_COUNT ($i)"
    exit 1    
fi

echo "Success"
exit 0