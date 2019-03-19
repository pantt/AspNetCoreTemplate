using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NetCoreFrame.Application.Samples;
using NetCoreFrame.Application.Samples.Dtos;
using NetCoreFrame.Entities;
using iCTR.TB.Framework.Filter;
using NetCoreFrame.WebApi.Authorize;

namespace NetCoreFrame.WebApi.Controllers
{
    /// <summary>
    /// SamplesController
    /// </summary>
    [Route("api/[controller]")]
    [EnableCors("AllowAllMethods")]
    //[BearerAuthorize]
    public class SamplesController : Controller
    {
        /// <summary>
        /// ISampleService
        /// </summary>
        private readonly ISampleService _service;
        /// <summary>
        /// 工作单元
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 构造函数 
        /// </summary>
        public SamplesController(ISampleService service, IUnitOfWork unitOfWork)
        {
            _service = service;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByPage([FromQuery] SamplePageInDto samplePageInDto)
        {
            var data = _service.GetSamplesByPage(samplePageInDto);
            return Ok(data);
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">idparmas</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] Guid id)
        {
            var entity = _service.GetSample(id);
            return Ok(entity);
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] SampleCreateDto dto)
        {
            var result = _service.CreateSample(dto);
            return Ok(result);
        }

        /// <summary>
        /// 全部更新
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update([FromBody] SampleUpdateDto value)
        {
            var entity = _service.UpdateSample(value);
            _unitOfWork.Save();
            return Ok(entity);
        }

        /// <summary>
        ///  部分更新
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="patchUpateDto">部分更新实体</param>
        [HttpPatch("{id}")]
        public IActionResult Patch([FromRoute] Guid id, [FromBody] SamplePatchUpdateDto patchUpateDto)
        {
            var entity = _service.PatchSample(id, patchUpateDto);
            _unitOfWork.Save();
            return Ok(entity);
        }

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            _service.DeleteSample(id);
            _unitOfWork.Save();
            return NoContent();
        }
        /// <summary>
        /// 示例
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>示例结果</returns>
        [HttpGet("getsample/{id}")]
        public IActionResult GetSampleAndUser([FromRoute]Guid id) {
            return Ok();

        }

        /// <summary>
        /// 示例：请求中抛出系统异常Exception
        /// </summary>
        /// <returns>返回标准输出内容</returns>
        [HttpGet("GetExampleSystemException")]
        public object GetExampleSystemException()
        {
            throw new Exception("系统异常示例");
            return new { aa = "aa", bb = "bb" };
        }

        /// <summary>
        /// 示例：请求中抛出自定义异常BussinessException
        /// </summary>
        /// <returns>返回标准输出内容</returns>
        [HttpGet("GetExampleBussinessException")]
        public object GetExampleBussinessException()
        {
            throw new BussinessException("BussinessExceptiontest");
            return new { aa = "aa", bb = "bb" };
        }
        
        /// <summary>
        /// 示例：返回Ok();
        /// </summary>
        /// <returns>返回标准输出内容</returns>
        [HttpGet("GetExampleStatusOk")]
        public object GetExampleStatusOk()
        {
            return Ok();
        }

        /// <summary>
        /// 示例：返回Ok("aaaa");
        /// </summary>
        /// <returns>返回标准输出内容</returns>
        [HttpGet("GetExampleStatusOkstr")]
        public object GetExampleStatusOkstr()
        {
            return Ok("aaaa");
        }

        /// <summary>
        /// 示例：返回Ok(new { aa="aa",bb="bb"});
        /// </summary>
        /// <returns>返回标准输出内容</returns>
        [HttpGet("GetExampleStatusOkObj")]
        public object GetExampleStatusOkObj()
        {
            return Ok(new { aa = "aa", bb = "bb" });
        }

        /// <summary>
        /// 示例：返回BadRequest("aaa");
        /// </summary>
        /// <returns>返回标准输出内容</returns>
        [HttpGet("GetExampleStatusBadStr")]
        public object GetExampleStatusBadStr()
        {
            return BadRequest("aaa");
        }

        /// <summary>
        /// 示例：返回BadRequest(new { aa = "aa", bb = "bb" })
        /// </summary>
        /// <returns>返回标准输出内容</returns>
        [HttpGet("GetExampleStatusBadObj")]
        public object GetExampleStatusBadObj()
        {
            return BadRequest(new { aa = "aa", bb = "bb" });
        }

        /// <summary>
        /// 示例：返回BadRequest()
        /// </summary>
        /// <returns>返回标准输出内容</returns>
        [HttpGet("GetExampleStatusBad")]
        public object GetExampleStatusBad()
        {
            return BadRequest();
        }

        /// <summary>
        /// 示例：返回类型为void
        /// </summary>
        /// <returns>返回标准输出内容</returns>
        [HttpGet("GetExampleVoid")]
        public void GetExampleVoid()
        {
        }
    }
}