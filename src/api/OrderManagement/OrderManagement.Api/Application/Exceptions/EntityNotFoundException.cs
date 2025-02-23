namespace OrderManagement.Api.Application.Exceptions;

internal class EntityNotFoundException(Type entityType, Guid id) 
    : Exception($"Entity of type {entityType.Name} with id {id} was not found.");