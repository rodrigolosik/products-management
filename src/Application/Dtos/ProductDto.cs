namespace Application.Dtos;

public record ProductDto (int Id, string? Name, double Price, int Quantity, string? Description);