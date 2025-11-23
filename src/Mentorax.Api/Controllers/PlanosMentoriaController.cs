using Microsoft.AspNetCore.Mvc;
using Mentorax.Api.Repositories.Interfaces;
using Mentorax.Api.Models.Dto;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Mentorax.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class PlanosMentoriaController : ControllerBase
    {
        private readonly IPlanoMentoriaRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<PlanosMentoriaController> _logger;

        public PlanosMentoriaController(
            IPlanoMentoriaRepository repo,
            IMapper mapper,
            ILogger<PlanosMentoriaController> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var entities = await _repo.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<PlanoMentoriaDto>>(entities);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar planos de mentoria");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return NotFound($"Plano de mentoria com ID {id} não encontrado");

                var dto = _mapper.Map<PlanoMentoriaDto>(entity);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter plano de mentoria com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] PlanoMentoriaDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entity = _mapper.Map<Mentorax.Api.Models.PlanoMentoria>(model);
                await _repo.AddAsync(entity);

                var dto = _mapper.Map<PlanoMentoriaDto>(entity);

                return CreatedAtAction(nameof(ObterPorId), new { id = entity.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar plano de mentoria");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] PlanoMentoriaDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existente = await _repo.GetByIdAsync(id);
                if (existente == null)
                    return NotFound($"Plano de mentoria com ID {id} não encontrado");

                var entity = _mapper.Map<Mentorax.Api.Models.PlanoMentoria>(model);
                entity.Id = id;

                await _repo.UpdateAsync(entity);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar plano de mentoria com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                var existente = await _repo.GetByIdAsync(id);
                if (existente == null)
                    return NotFound($"Plano de mentoria com ID {id} não encontrado");

                await _repo.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir plano de mentoria com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
