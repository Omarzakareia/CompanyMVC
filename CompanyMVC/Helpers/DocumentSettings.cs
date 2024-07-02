namespace CompanyMVC.Helpers
{
    public static class DocumentSettings
    {
        // upload
        public static string UploadFile(IFormFile file , string FolderName)
        {
            //string FolderPath = Directory.GetCurrentDirectory() +"\\wwwroot\\Files\\" + FolderName;
            //1, Get Located Folder Path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);
            //2. Get File Name and Make it Unique

            string FileName = $"{file.FileName}";
            //3. Get File Path[Folder Path + FileName] 
            string FilePath = Path.Combine(FolderPath, FileName);
            //4. Save File As Streams
            using var Fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(Fs);
            //5. Return File Name
            return FileName;
        }
        //Delete
        public static void DeleteFile(string FileName , string FolderName)
        {
            // 1. Get File Path 
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName , FileName);
            // 2. Check if File Exists Or Not
            if(File.Exists(FilePath))
            {
                //if exists Remove it
                File.Delete(FilePath);
            } 

        }


    }
}
