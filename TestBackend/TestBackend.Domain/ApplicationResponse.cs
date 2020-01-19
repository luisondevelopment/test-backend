using System.Collections.Generic;

namespace TestBackend.Domain
{
    public class ApplicationResponse
    {
        public ApplicationResponse(object data, List<string> errors)
        {
            Data = data;
            Errors = errors;
        }

        public object Data { get; set; }
        public List<string> Errors { get; set; }

        public bool Ok()
        {
            return Errors.Count == 0;
        }
    }
}
