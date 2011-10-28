using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Data.Linq;

using HRAutomation.DAL;

namespace _HRAutomation.resources.classes
{
    public class FileReport
    {
        public VendorLite VendorObject { get; private set; }
        public string UserID { get; private set; }

        /// <summary>
        /// Our constructor requires a VendorLIte object for an obvious reason...
        /// We can't report with a transfer object....
        /// </summary>
        /// <param name="vl"></param>
        public FileReport(VendorLite vl) 
        {
            this.VendorObject = vl;
            this.UserID = vl.UserID;
        }


        /// <summary>
        /// Actually creates the instance of the object for saving
        /// relevant items to the database and their corresponding files.
        /// </summary>
        public void SubmitReport()
        {

                var IR = new InterfaceRecord();
                IR.WasTest = this.VendorObject.WasTest;
                IR.PerformedBy = this.UserID;
                IR.VendorName = this.VendorObject.VendorName;
                IR.VendorFileName = Path.GetFileName(this.VendorObject.cleanUpItems.EncryptedFilePath);
                IR.EncryptedFileName = Path.GetFileName(this.VendorObject.cleanUpItems.EncryptedFilePath);
                    var bA = new System.Data.Linq.Binary(FileReport.ReturnFileBytes(this.VendorObject.cleanUpItems.EncryptedFilePath));
                IR.EncryptedFile = bA;
                IR.HrSystemFileName = Path.GetFileName(this.VendorObject.cleanUpItems.TxtWorkingFilePath);
                     var bA2 = new System.Data.Linq.Binary(FileReport.ReturnFileBytes(this.VendorObject.cleanUpItems.TxtWorkingFilePath));
                IR.HrSystemFile = bA2;
                IR.Message = this.VendorObject.TransmissionReport;
                IR.DateOfAction = DateTime.Now;

                //Plop it in!!
                DatabaseHelper.Insert<InterfaceRecord>(IR);
            
        }

        /// <summary>
        /// Returns the 
        /// </summary>
        /// <param name="filePathway"></param>
        /// <returns></returns>
        private static byte[] ReturnFileBytes(string filePathway)
        {
            FileStream fs = File.OpenRead(filePathway);
            byte[] data = FileReport.ReadFully(fs, 0);
            fs.Flush();
            fs.Close();
           
            return data;
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        private static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }


    }
}
