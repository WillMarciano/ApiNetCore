using AutoMapper;
using MarcianoIO.Api.Controllers;
using MarcianoIO.Api.Extensions;
using MarcianoIO.Api.ViewModels;
using MarcianoIO.Business.Interfaces;
using MarcianoIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MarcianoIO.Api.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/produtos")]
    public class ProdutosController : MainController
    {
        private readonly IProdutoRepository _produtoRepsitory;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProdutosController(IProdutoRepository produtoRepsitory,
                                  IProdutoService produtoService,
                                  IMapper mapper,
                                  INotificador notificador,
                                  IUser user,
                                  IHttpContextAccessor httpContextAccessor) : base(notificador, user)
        {
            _produtoRepsitory = produtoRepsitory;
            _produtoService = produtoService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
            => _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepsitory.ObterProdutosFornecedores());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null) return NotFound();
            return produtoViewModel;
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> Adicionar(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;

            if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
                return CustomResponse(produtoViewModel);

            produtoViewModel.Imagem = imagemNome;
            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [HttpPost("adicionar")]
        public async Task<ActionResult<ProdutoViewModel>> AdicionarAlternativo(ProdutoImagemViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivoAlternativo(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                return CustomResponse(ModelState);
            }

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Atualizar")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
            {
                NotificarErro("Os ids informados não são iguais!");
                return CustomResponse();
            }

            var produtoAtualizacao = await ObterProduto(id);
            produtoViewModel.Imagem = produtoAtualizacao.Imagem;
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (produtoViewModel.ImagemUpload != null)
            {
                var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
                if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
                {
                    return CustomResponse(ModelState);
                }
                produtoAtualizacao.Imagem = imagemNome;
            }

            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtoRepsitory.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));
            return CustomResponse(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Excluir(Guid id)
        {
            var produto = await ObterProduto(id);
            if (produto == null) return NotFound();
            await _produtoService.Remover(id);
            return CustomResponse(produto);
        }

        //Teste Imagem form-data
        [RequestSizeLimit(40000000)]
        //[DisableRequestSizeLimit]
        [HttpPost("imagem")]
        public ActionResult AdicionarImagem(IFormFile file)
        {
            return Ok(file);
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {
            if (string.IsNullOrEmpty(arquivo))
            {
                //ModelState.AddModelError(string.Empty, "Forneça uma imagem para este produto!");
                NotificarErro("Forneça uma imagem para este produto!");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(arquivo);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgNome);
            if (System.IO.File.Exists(filePath))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);
            return true;
        }

        private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                NotificarErro("Forneça uma imagem para este produto!");
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
            => _mapper.Map<ProdutoViewModel>(await _produtoRepsitory.ObterProdutoFornecedor(id));
    }
}
