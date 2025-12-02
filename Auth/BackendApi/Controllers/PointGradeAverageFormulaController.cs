using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BackendApi.Core.Models;
using BackendApi.Core.Models.Dto;
using BackendApi.Context;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointGradeAverageFormulaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PointGradeAverageFormulaController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{gradeFormulaId}")]
        public async Task<IActionResult> GetByGradeFormula(int gradeFormulaId)
        {
            var formula = await _context.PointGradeAverageFormulas
                .FirstOrDefaultAsync(x => x.GradeFormulaId == gradeFormulaId);

            if (formula == null)
                return NotFound();

            return Ok(_mapper.Map<PointGradeAverageFormulaDto>(formula));
        }

        [HttpPost]
        public async Task<IActionResult> Create(PointGradeAverageFormulaDto dto)
        {
            var entity = _mapper.Map<PointGradeAverageFormula>(dto);
            _context.PointGradeAverageFormulas.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<PointGradeAverageFormulaDto>(entity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PointGradeAverageFormulaDto dto)
        {
            var entity = await _context.PointGradeAverageFormulas.FindAsync(id);
            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<PointGradeAverageFormulaDto>(entity));
        }
    }
}
