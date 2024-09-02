﻿using FinShark.Server.Dtos.Comment;
using FinShark.Server.Interfaces;
using FinShark.Server.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly InterfaceCommentRepository _interfaceComment;
        private readonly InterfaceStockRepository _interfaceStock;
        public CommentController(InterfaceCommentRepository interfaceCommentRepository, InterfaceStockRepository interfaceStock)
        {
            _interfaceComment = interfaceCommentRepository;
            _interfaceStock = interfaceStock;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Basicamente esto va a validar todo lo que usamos en Data Annotations.
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _interfaceComment.GetAllAsync();
            var commentsDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentsDto);

        }

        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _interfaceComment.GetByIdAsync(id);

            if(comment == null)
                return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _interfaceStock.StockExists(stockId))
                return BadRequest("Stock does not exist");

            var commentModel = requestDto.ToCommentFromCreateComment(stockId);
            await _interfaceComment.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateCommentRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _interfaceComment.UpdateAsync(id, requestDto);

            if(comment == null)
                return NotFound("Comment not found");

            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _interfaceComment.DeleteAsync(id);

            if(comment == null)
                return NotFound("Comment doesn't exist");

            return NoContent();
        }
        
    }
}
