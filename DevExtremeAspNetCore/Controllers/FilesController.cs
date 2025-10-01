using Microsoft.AspNetCore.Mvc;

namespace DevExtremeAspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public FilesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("GetExcel")]
        public async Task<IActionResult> GetExcel(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return BadRequest("File URL is required.");

            try
            {
                // Gọi request đến Firebase
                var response = await _httpClient.GetAsync(fileUrl);

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Không tải được file từ Firebase.");

                var bytes = await response.Content.ReadAsByteArrayAsync();

                // Trả về cho client (SheetJS đọc được)
                return File(
                    bytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "file.xlsx"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi proxy: {ex.Message}");
            }
        }
    }
}
