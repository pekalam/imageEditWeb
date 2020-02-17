using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageEdit.Core.Domain
{
    public class EditTaskAction
    {
        private static readonly string[] AllowedActions = new string[]{"convert"};

        public string ActionName { get; }
        public Dictionary<string, string> Params { get; }

        public EditTaskAction(string actionName, Dictionary<string, string> @params)
        {
            if (!AllowedActions.Contains(actionName.ToLower()))
            {
                throw new ArgumentException($"Invalid actionName {actionName}");
            }
            ActionName = actionName;
            Params = @params;
        }
    }
}