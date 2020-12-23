using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DiaryAPI.Entities;
using DiaryAPI.Repositories.Interfaces;
using EventBusRabbitMq.Common;
using EventBusRabbitMq.Events;
using EventBusRabbitMq.Producer;
using Microsoft.AspNetCore.Mvc;

namespace DiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDiaryNoteRepository _repository;
        private readonly EventBusRabbitMqProducer _eventBus;

        public DiaryController(IMapper mapper, IDiaryNoteRepository repository, EventBusRabbitMqProducer eventBus)
        {
            _mapper = mapper;
            _eventBus = eventBus;
            _repository = repository;
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<DiaryNote>), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<IEnumerable<DiaryNote>>> GetDiaryNotes()
        //{
        //    var diaryNotes = await _repository.GetDiaryNotes();
        //    return Ok(diaryNotes);
        //}


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DiaryNote>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDiaryNotes()
        {
            var diaryNotes = await _repository.GetDiaryNotes();
            return Ok(diaryNotes);
        }


        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<DiaryNote>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DiaryNote>>> Create([FromBody] DiaryNote diaryNote)
        {
            await _repository.Create(diaryNote);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DiaryNoteQueue([FromBody] DiaryNote diaryNote)
        {
            var eventMsg =  _mapper.Map<SaveDiaryNoteEvent>(diaryNote);
            eventMsg.RequestID = Guid.NewGuid();

            try
            {
                _eventBus.PublishSaveDiaryNoteEvent(EventBusConsts.DiaryNoteQueue, eventMsg);
            }
            catch (Exception)
            {

            }

            return Accepted();
        }
    }
}
