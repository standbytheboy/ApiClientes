using ApiClientes.Data;
using ApiClientes.Models;
using ApiClientes.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiClientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _contextFromDb;
        private readonly ClienteService _clienteService;
        public ClienteController(AppDbContext contextFromDb, ClienteService clienteService)
        {
            _contextFromDb = contextFromDb;
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _contextFromDb.Clientes
            .Select(c => new
            {
                // objeto para limitar o que aparece em tela
                c.Id,
                c.Nome,
                c.Email,
                c.Telefone,
                c.DataNascimento
            })
            .ToListAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Cliente? cliente = await _contextFromDb.Clientes.Include(c => c.Enderecos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound($"Cliente com o id {id} não foi encontrado");
            }
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _clienteService.ValidarCpfAsync(cliente.Cpf)) { throw new Exception("CPF inválido"); }
            _contextFromDb.Clientes.Add(cliente);
            await _contextFromDb.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("O ID do cliente na URL não corresponde ao ID no corpo da solicitação.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingCliente = await _contextFromDb.Clientes.FindAsync(id);
            if (existingCliente == null)
            {
                return NotFound($"Cliente com o id {id} não foi encontrado");
            }
            
            existingCliente.Nome = cliente.Nome;
            existingCliente.Cpf = cliente.Cpf;
            existingCliente.RG = cliente.RG;
            existingCliente.Email = cliente.Email;
            existingCliente.Telefone = cliente.Telefone;
            existingCliente.DataNascimento = cliente.DataNascimento;
            existingCliente.Enderecos = cliente.Enderecos;
            existingCliente.DataUltimaAtualizacao = DateTime.UtcNow;
            existingCliente.Ativo = cliente.Ativo;
            _contextFromDb.Clientes.Update(existingCliente);
            await _contextFromDb.SaveChangesAsync();
            return Ok(existingCliente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Cliente? cliente = await _contextFromDb.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound($"Endereço com o id {id} não foi encontrado");
            }
            _contextFromDb.Clientes.Remove(cliente);
            await _contextFromDb.SaveChangesAsync();
            return Ok("Cliente deletado com sucesso!");
        }
    }
}
