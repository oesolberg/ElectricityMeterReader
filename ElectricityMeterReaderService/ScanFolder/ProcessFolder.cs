using System.Collections.Generic;
using System.IO;
using System.Linq;
using ElectricityMeterReaderService.ImageHandlingLogic;

namespace ElectricityMeterReaderService.ScanFolder
{
    public class ProcessFolder
    {
        public void Execute(string folderToWatch,string filefilter)
        {
            //Fetch all files in the folder
            
            var fileOp = new FetchFilesFromWatchedFolder(folderToWatch, filefilter);
            var diskFileList = fileOp.Execute();

            //Fetch all files in the database.
            var dbOp=new DataStorage.DataHandling.GetAllImageNames();
            var dbFilesList=dbOp.Execute();

            //Compare and fetch the missing files if any are missing
            var filesToFetch = FindAnyMissingFiles(dbFilesList, diskFileList);

            ProcessEachMissingFile(filesToFetch);
        }

        private void ProcessEachMissingFile(List<MissingFileData> filesToFetch)
        {
            var imageHandler = new ElectricityImageHandler();
            var dbStore = new DataStorage.DataStorageHandler();
            foreach (var fileToProcess in filesToFetch)
            {
                
                var imageData = imageHandler.DoImageProcessing(fileToProcess.Filepath);
                if (imageData != null)
                {
                    dbStore.DoStorage(imageData);
                }
                else
                {
                    dbStore.StoreFileInBlackList(fileToProcess.Filepath);
                }
            }
        }

        private List<MissingFileData> FindAnyMissingFiles(List<string> dbFilesList, List<string> diskFileList)
        {
            var missingFileList=new List<MissingFileData>();
            foreach (var filepath in diskFileList)
            {
                if (!dbFilesList.Contains(Path.GetFileName(filepath)))
                {
                    missingFileList.Add(new MissingFileData() {Filepath= filepath,CreatedDateTime=File.GetLastWriteTime(filepath)});
                }
            }
            if (missingFileList.Any())
            {
                missingFileList= missingFileList.OrderBy(f=>f.CreatedDateTime).ToList();
            }
            return missingFileList;
        }
    }
}