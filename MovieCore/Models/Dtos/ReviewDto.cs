using System.ComponentModel.DataAnnotations;

namespace MovieCore.Models.Dtos
{
    public record ReviewDto(int Id, string ReviewerName, string Comment, [Range(1, 5)] int Rating);
}
