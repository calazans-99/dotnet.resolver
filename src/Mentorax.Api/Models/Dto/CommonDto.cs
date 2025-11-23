
using System;
using System.Collections.Generic;

namespace Mentorax.Api.Models.Dto
{
    /// <summary>
    /// DTO para resultado paginado
    /// </summary>
    /// <typeparam name="T">Tipo dos itens da página</typeparam>
    public class PagedResultDto<T>
    {
        /// <summary>
        /// Itens da página atual
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Número da página atual
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Tamanho da página
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total de itens
        /// </summary>
        public long TotalItems { get; set; }

        /// <summary>
        /// Total de páginas
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        /// <summary>
        /// Indica se há página anterior
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Indica se há próxima página
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// Links HATEOAS para navegação
        /// </summary>
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }

    /// <summary>
    /// DTO para resposta de erro
    /// </summary>
    public class ErrorResponseDto
    {
        /// <summary>
        /// Código do erro
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Mensagem do erro
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Detalhes adicionais do erro
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Timestamp do erro
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Lista de erros de validação
        /// </summary>
        public Dictionary<string, string[]>? ValidationErrors { get; set; }
    }
}
