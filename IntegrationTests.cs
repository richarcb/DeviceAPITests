using Microsoft.AspNetCore.Mvc.Testing;
using HuddlyAssignment;
using HuddlyAssignment.Models;
using Xunit;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace HuddlyAssignment.Tests
{
    public class ControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5290";
        public ControllerTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
        }
        [Fact]
        public async Task Get_Device_ReturnsValues()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/api/device/VU4608");
            response.EnsureSuccessStatusCode();

            var deviceJsonString = await response.Content.ReadAsStringAsync();
            Device device = JsonConvert.DeserializeObject<Device>(deviceJsonString);
            Assert.NotNull(device);
            Assert.Equal(device.DeviceId, "VU4608");
        }
        [Fact]
        public async Task Get_DeviceList_ReturnsValues()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/api/device");
            response.EnsureSuccessStatusCode();

            var devicesJsonString = await response.Content.ReadAsStringAsync();
            List<Device> deviceList = JsonConvert.DeserializeObject<List<Device>>(devicesJsonString);
            Assert.NotNull(deviceList);
            
        }

        [Fact]
        public async Task Post_Device_Check_If_Exists()
        {

            var device = new Device
            {

                DeviceId = "TESTID",
                DeviceModel = "TESTMODEL",
                Room = "TESTROOM",
                Organization = "TESTORG",
                DateAdded = "20231311"
            };

            StringContent jsonPayload = new StringContent(JsonConvert.SerializeObject(device), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseUrl + "/api/device", jsonPayload);

            response.EnsureSuccessStatusCode();

            var getResponse = await _httpClient.GetAsync(_baseUrl + "/api/device/TESTID");
            Assert.NotNull(getResponse);

            await _httpClient.DeleteAsync(_baseUrl + "/api/device/TESTID");


        }
        [Fact]
        public async Task Delete_Check_If_Device_Is_Added_Then_Deleted()
        {
            var device = new Device
            {

                DeviceId = "TESTDELETE",
                DeviceModel = "TESTMODEL",
                Room = "TESTROOM",
                Organization = "TESTORG",
                DateAdded = "20231311"
            };

            StringContent jsonPayload = new StringContent(JsonConvert.SerializeObject(device), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseUrl + "/api/device", jsonPayload);

            response.EnsureSuccessStatusCode();

            var getResponse = await _httpClient.GetAsync(_baseUrl + "/api/device/TESTDELETE");
            Assert.NotNull(getResponse);

            var deleteResponse = await _httpClient.DeleteAsync(_baseUrl + "/api/device/TESTDELETE");
            Assert.NotNull(deleteResponse);

            getResponse = await _httpClient.GetAsync(_baseUrl + "/api/device/TESTDELETE");
            Assert.Equal(HttpStatusCode.NoContent, getResponse.StatusCode);
        }
    }
}
