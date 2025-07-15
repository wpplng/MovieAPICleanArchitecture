using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Dtos
{
    public record ReviewDto(int Id, string ReviewerName, string Comment, [Range(1,5)] int Rating);
}
