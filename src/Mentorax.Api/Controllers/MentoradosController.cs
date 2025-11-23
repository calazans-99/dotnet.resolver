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
    public class MentoradosController : ControllerBase
    {
        private readonly IMentoradoRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<MentoradosController> _logger;

        public MentoradosController(
            IMentoradoRepository repo,
            IMapper mapper,
            ILogger<MentoradosController> logger)
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
                var dtos = _mapper.Map<IEnumerable<MentoradoDto>>(entities);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar mentorados");
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
                    return NotFound($"Mentorado com ID {id} não encontrado");

                var dto = _mapper.Map<MentoradoDto>(entity);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter mentorado com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] MentoradoDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entity = _mapper.Map<Mentorax.Api.Models.Mentorado>(model);
                await _repo.AddAsync(entity);

                var dto = _mapper.Map<MentoradoDto>(entity);

                return CreatedAtAction(nameof(ObterPorId), new { id = entity.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar mentorado");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] MentoradoDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existente = await _repo.GetByIdAsync(id);
                if (existente == null)
                    return NotFound($"Mentorado com ID {id} não encontrado");

                var entity = _mapper.Map<Mentorax.Api.Models.Mentorado>(model);
                entity.Id = id;

                await _repo.UpdateAsync(entity);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar mentorado com ID {Id}", id);
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
                    return NotFound($"Mentorado com ID {id} não encontrado");

                await _repo.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir mentorado com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
