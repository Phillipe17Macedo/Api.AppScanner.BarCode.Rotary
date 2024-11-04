using Api.AppScanner.BarCode.Rotary.COMMON.Models;
using Api.AppScanner.BarCode.Rotary.EF.DataContext;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.AppScanner.BarCode.Rotary.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CodigoBarrasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CodigoBarrasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("BuscarCadastrosScannerBarCode")]
        public async Task<IActionResult> BuscarCadastrosScannerBarCode()
        {
            var scanners = await _context.CodigoBarrasModel.ToListAsync();

            return Ok(scanners);
        }

        [HttpPost("CadastrarScannerBarCode")]
        public async Task<IActionResult> CadastrarScannerBarCode(CodigoBarrasModel codigoBarras)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Erro de validação.",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                await _context.CodigoBarrasModel.AddAsync(codigoBarras);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    sucess = true,
                    message = "Código de Barras cadastrado com sucesso!",
                    data = codigoBarras
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro interno no servidor.",
                    error = ex.Message
                });
            }
        }

        [HttpPut("EditarCadastroScannerBarCode/{id}")]
        public async Task<IActionResult> EditarCadastroScannerBarCode(int id, CodigoBarrasModel scanner)
        {
            if (!ModelState.IsValid || id != scanner.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Erro de validação.",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var resultado = await _context.CodigoBarrasModel.FindAsync(id);
                if (resultado == null)
                {
                    return NotFound(new
                    {
                        sucess = false,
                        message = "Cadastro não encontrado."
                    });
                }

                resultado.Tipo = scanner.Tipo;
                resultado.Codigo = scanner.Codigo;
                resultado.DataLeitura = scanner.DataLeitura;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Cadastro atualizado com sucesso."
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro interno no servidor.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("RemoverCadastroScannerBarCode/{id}")]
        public async Task<IActionResult> RemoverCadastroScannerBarCode(int id)
        {
            try
            {
                var resultado = await _context.CodigoBarrasModel.FindAsync(id);
                if (resultado == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Cadastro não encontrado."
                    });
                }

                _context.CodigoBarrasModel.Remove(resultado);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Cadastro Scanner removido com sucesso."
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro interno no servidor.",
                    error = ex.Message
                });
            }
        }






    }
}
