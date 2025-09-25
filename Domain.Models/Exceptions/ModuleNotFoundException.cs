using System;

namespace Domain.Models.Exceptions
{
    public class ModuleNotFoundException : Exception
    {
        public ModuleNotFoundException(Guid moduleId)
            : base($"Module with id '{moduleId}' was not found.")
        {
        }
    }
}
