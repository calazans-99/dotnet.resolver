using System.ComponentModel.DataAnnotations;

namespace Mentorax.Api.Models.Dto
{
    /// <summary>
    /// DTO para resposta de plano de mentoria
    /// </summary>
    public class PlanoMentoriaDto
    {
        /// <summary>
        /// Identificador único do plano de mentoria
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Identificador do mentorado associado ao plano
        /// </summary>
        [Required(ErrorMessage = "O ID do mentorado é obrigatório")]
        public Guid MenteeId { get; set; }

        /// <summary>
        /// Título do plano de mentoria
        /// </summary>
        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(255, ErrorMessage = "Título deve ter no máximo 255 caracteres")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Resumo/descrição geral do plano
        /// </summary>
        [Required(ErrorMessage = "Resumo é obrigatório")]
        [StringLength(1000, ErrorMessage = "Resumo deve ter no máximo 1000 caracteres")]
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Agenda semanal do plano em formato JSON
        /// </summary>
        [Required(ErrorMessage = "Agenda semanal é obrigatória")]
        public string WeeklyScheduleJson { get; set; } = string.Empty;

        /// <summary>
        /// Data de criação do plano
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Links HATEOAS associados ao recurso
        /// </summary>
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
