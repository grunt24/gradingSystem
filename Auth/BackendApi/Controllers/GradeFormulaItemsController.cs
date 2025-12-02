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
    public class GradeFormulaItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GradeFormulaItemsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET BY FORMULA
        [HttpGet("formula/{formulaId}")]
        public async Task<IActionResult> GetItemsByFormula(int formulaId)
        {
            var items = await _context.GradeFormulaItems
                .Where(i => i.GradeFormulaId == formulaId)
                .ToListAsync();

            var dto = _mapper.Map<IEnumerable<GradeFormulaItemDto>>(items);
            return Ok(dto);
        }

        // CREATE ITEM
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] GradeFormulaItemDto dto)
        {
            if (dto == null)
                return BadRequest();

            var entity = _mapper.Map<GradeFormulaItem>(dto);

            _context.GradeFormulaItems.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<GradeFormulaItemDto>(entity);
            return Ok(result);
        }

        // UPDATE ITEM
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] GradeFormulaItemDto dto)
        {
            if (dto == null || dto.Id != id)
                return BadRequest();

            var existing = await _context.GradeFormulaItems.FindAsync(id);

            if (existing == null)
                return NotFound();

            _mapper.Map(dto, existing);

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<GradeFormulaItemDto>(existing));
        }

        // DELETE ITEM
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var existing = await _context.GradeFormulaItems.FindAsync(id);

            if (existing == null)
                return NotFound();

            _context.GradeFormulaItems.Remove(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
