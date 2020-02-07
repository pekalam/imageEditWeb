using System.Collections.Generic;

namespace ImageEdit.Core.Domain
{
    public class EditTaskAction
    {
        public string ActionName { get; }
        public Dictionary<string, string> Params { get; }

        public EditTaskAction(string actionName, Dictionary<string, string> @params)
        {
            ActionName = actionName;
            Params = @params;
        }
    }
}