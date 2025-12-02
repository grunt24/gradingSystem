using AutoMapper;
using BackendApi.Context;
using BackendApi.Core.Models;
using BackendApi.Core.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeFormulasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GradeFormulasController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var formulas = await _context.GradeFormulas
                .Include(f => f.Items)
                .ToListAsync();

            var dto = _mapper.Map<IEnumerable<GradeFormulaDto>>(formulas);
            return Ok(dto);
        }

        // GET ONE
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var formula = await _context.GradeFormulas
                .Include(f => f.Items)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (formula == null)
                return NotFound();

            var dto = _mapper.Map<GradeFormulaDto>(formula);
            return Ok(dto);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GradeFormulaDto dto)
        {
            if (dto == null)
                return BadRequest();

            var formula = _mapper.Map<GradeFormula>(dto);

            _context.GradeFormulas.Add(formula);
            await _context.SaveChangesAsync();

            var returnDto = _mapper.Map<GradeFormulaDto>(formula);
            return CreatedAtAction(nameof(GetById), new { id = formula.Id }, returnDto);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GradeFormulaDto dto)
        {
            if (dto == null || dto.Id != id)
                return BadRequest();

            var existing = await _context.GradeFormulas
                .Include(f => f.Items)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (existing == null)
                return NotFound();

            _mapper.Map(dto, existing);

            await _context.SaveChangesAsync();

            var returnDto = _mapper.Map<GradeFormulaDto>(existing);
            return Ok(returnDto);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.GradeFormulas
                .Include(f => f.Items)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (existing == null)
                return NotFound();

            _context.GradeFormulaItems.RemoveRange(existing.Items);
            _context.GradeFormulas.Remove(existing);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
