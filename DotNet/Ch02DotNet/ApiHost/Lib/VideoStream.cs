using System.Diagnostics;

namespace ApiHost.Lib;

public static class VideoStream
{
    public static async Task<bool> WriteContentToStream(this
        Stream outputStream, string filePath)
    {
        //path of file which we have to read//
        //var filePath = httpContext.Server.MapPath("~/MicrosoftBizSparkWorksWithStartups.mp4");
        //here set the size of buffer, you can set any size
        int bufferSize = 1000;
        byte[] buffer = new byte[bufferSize];
        //here we re using FileStream to read file from server//
        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int totalSize = (int)fileStream.Length;
                /*here we are saying read bytes from file as long as total size of file

                is greater then 0*/
                while (totalSize > 0)
                {
                    int count = totalSize > bufferSize ? bufferSize : totalSize;
                    //here we are reading the buffer from orginal file
                    int sizeOfReadedBuffer = fileStream.Read(buffer, 0, count);
                    //here we are writing the readed buffer to output//
                    await outputStream.WriteAsync(buffer, 0, sizeOfReadedBuffer);
                    //and finally after writing to output stream decrementing it to total size of file.
                    totalSize -= sizeOfReadedBuffer;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return false;
    }
}