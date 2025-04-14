using System;

namespace Beyond_PruebaTecnica_ConsoleApp.Exceptions
{
    public abstract class TodoListExceptions(string message) 
        : Exception(message)
    {
    }

    public class InvalidCategoryException : TodoListExceptions
    {
        public InvalidCategoryException()
            : base("La categoría especificada no es válida.") 
        {
        }
    }

    public class InvalidProgressException : TodoListExceptions
    {
        public InvalidProgressException()
            : base("El porcentaje debe ser mayor que 0 y menor que 100.") { }
    }

    public class InvalidProgressDateException : TodoListExceptions
    {
        public InvalidProgressDateException()
            : base("La fecha debe ser mayor que la última registrada.") 
        {
        }
    }

    public class ProgressOverflowException : TodoListExceptions
    {
        public ProgressOverflowException()
            : base("No se puede exceder el 100% de progreso.") 
        {
        }
    }

    public class UpdateNotAllowedException : TodoListExceptions
    {
        public UpdateNotAllowedException()
            : base("No se puede actualizar un item con más del 50% completado.") 
        {
        }
    }

    public class RemoveNotAllowedException : TodoListExceptions
    {
        public RemoveNotAllowedException()
            : base("No se puede eliminar un item con más del 50% completado.") 
        {
        }
    }

    public class ItemNotFoundException : TodoListExceptions
    {
        public ItemNotFoundException(int id)
            : base($"No existe un item con Id={id}.") 
        {
        }
    }
}
