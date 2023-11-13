using HuddlyAssignment.Data;
using HuddlyAssignment.Models;
using HuddlyAssignment.Controllers;
using HuddlyAssignment.Dtos;

namespace HuddlyAssignment.Tests;

public class DeviceRepoTests
{
    private string TestFolder = "C:\\Users\\richcb\\source\\repos\\HuddlyAssignment.Tests\\Data\\";

    [Fact]
    public void CreateDevice_Should_WriteToCsvFile()
    {
        var filePath = TestFolder + "dataset_test.csv";
        var deviceRepo = new DeviceRepo(filePath);

        var deviceInput = new Device
        {
            DeviceId = "1",
            DeviceModel = "Model1",
            Room = "Room1",
            Organization = "Org1",
            DateAdded="20231211"
        };

        deviceRepo.CreateDevice(deviceInput);


        var deviceOutput = deviceRepo.FetchDeviceById(deviceInput.DeviceId);
        Assert.Equal(deviceInput.DeviceId, deviceOutput.DeviceId);
        
    }
    [Fact]
    public void DeleteDeviceById_Should_RemoveDeviceFromCsvFile()
    {
        var filePath = TestFolder + "dataset_test.csv";
        var repo = new DeviceRepo(filePath);

        var initialDevices = new List<Device>
        {
            new Device  { 
                DeviceId = "2", 
                DeviceModel = "Model2", 
                Room = "Room2", 
                Organization = "Org2", 
                DateAdded = "20231112"
            }
        };
        
        repo.CreateDevices(initialDevices);

        repo.DeleteDeviceById("2");

        var remainingDevices = repo.FetchDevices();
        Assert.DoesNotContain(remainingDevices, d => d.DeviceId == "2");
    }
}