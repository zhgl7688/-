using Fruit.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fruit.Web.Controllers
{
    /// <summary>
    /// 提供对文件上传、下载、列表等功能的支持控制器
    /// </summary>
    public class FileController : Controller
    {
        public class FileSerial
        {
            public int FileId { get; set; }
            public int? Serial { get; set; }
        }

        [HttpPost]
        public JsonResult Get(int id)
        {
            //string fileIdText = Request.Form["fileId"];
            //int fileId = 0;
            //int.TryParse(fileIdText, out fileId);
            using (var db = new SysContext())
            {
                return Json(db.sys_file.Where(f => f.FileId == id).ToList());
            }
        }

        // /File/Upload
        [HttpPost]
        public JsonResult Upload()
        {
            string rootPath = Server.MapPath("~/Upload");
            string path = Request.Form["path"];
            bool replace = bool.Parse(Request.Form["replace"]);
            if(!string.IsNullOrEmpty(path)){
                path = Fruit.Data.FruitExpression.Replace(path);
                var targetPath = Server.MapPath("~/Upload/" + path);
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }
            }
            string saveDir = rootPath;
            string fileIdText = Request.Form["fileId"];
            int fileId = 0;
            int.TryParse(fileIdText, out fileId);

            List<sys_file> files = new List<sys_file>();

            foreach (var file in Request.Files.GetMultiple("file"))
            {
                if (file.ContentLength > 1)
                {
                    var saveName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
                    var savePath = System.IO.Path.Combine(rootPath, path, saveName);
                    file.SaveAs(savePath);
                    files.Add(new sys_file { FileId = fileId, FileName = System.IO.Path.GetFileName(file.FileName), Path = "/Upload/" + path + "/" + saveName, CreateDate = DateTime.Now, CreatePerson = Fruit.Data.FruitExpression.Replace("{U.UserCode}") });
                }
            }

            if (files.Count > 0)
            {
                // 需要进行保存操作
                using (var db = new SysContext())
                {
                    if (fileId == 0)
                    {
                        fileId = new SysSerialServices().GetNewSerial(db, "sys_file");
                    }
                    else if (replace)
                    {
                        db.Database.ExecuteSqlCommand("delete from sys_file where FileId = @FileId", new System.Data.SqlClient.SqlParameter("@FileId", fileId));
                    }
                    var serial = db.Database.SqlQuery<int>("select top 1 Serial from sys_file where FileId = @FileId order by Serial desc", new System.Data.SqlClient.SqlParameter("@FileId", fileId)).FirstOrDefault();
                    foreach (var file in files)
                    {
                        file.FileId = fileId;
                        file.Serial = ++serial;
                        db.sys_file.Add(file);
                    }
                    db.SaveChanges();
                }
            }

            return Json(new { fileId, files });
        }
        [HttpPost]
        public JsonResult UploadBase64(string base64Data,string filename)
        {  
            string rootPath = Server.MapPath("~/Upload/image/");
            var saveName = filename;
            var savePath=rootPath + saveName;
            byte[] arr2 = Convert.FromBase64String(base64Data);
            using (MemoryStream ms2 = new MemoryStream(arr2))
            {
                System.Drawing.Bitmap bmp2 = new System.Drawing.Bitmap(ms2);
                ////只有把当前的图像复制一份，然后把旧的Dispose掉，那个文件就不被锁住了，
                ////这样就可以放心覆盖原始文件，否则GDI+一般性错误(A generic error occurred in GDI+)
                //System.Drawing.Bitmap bmpNew = new System.Drawing.Bitmap(bmp2);
                //bmp2.Dispose();
                //bmp2 = null;
                var ext = savePath.Substring(savePath.Length - 3).ToLower();
                switch (ext)
                {
                    case "jpg": bmp2.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "bmp": bmp2.Save(savePath, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case "gif": bmp2.Save(savePath, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "png": bmp2.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case "ico": bmp2.Save(savePath, System.Drawing.Imaging.ImageFormat.Icon);
                        break;
                    
                }
                bmp2.Dispose();
            }
            var success = System.IO.File.Exists(savePath);
            var result = new
            {
                success=success,
                filename = saveName
            };
            return Json(result );
        }


        [HttpPost]
        public JsonResult Delete(FileSerial obj)
        {
            //string fileIdText = obj.GetValue("fileId").ToString();
            //string serialText = obj.GetValue("serial").ToString();
            //int fileId = 0, serial = 0;
            //int.TryParse(fileIdText, out fileId);
            //int.TryParse(serialText, out serial);
            var fileId = obj.FileId;
            var serial = obj.Serial;

            if (fileId > 0 && serial > 0)
            {
                try
                {
                    using (var db = new SysContext())
                    {
                        var file = db.sys_file.Find(fileId, serial);
                        if (file != null)
                        {
                            var path = Server.MapPath("~" + file.Path);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            db.sys_file.Remove(file);
                            db.SaveChanges();
                        }
                    }
                }
                catch { }
            }
            else
            {
                return Json(false);
            }

            return Json(true);
        }

        public ActionResult Show(FileSerial obj)
        {
            //string fileIdText = obj.GetValue("fileId").ToString();
            //string serialText = obj.GetValue("serial").ToString();
            //int fileId = 0, serial = 0;
            //int.TryParse(fileIdText, out fileId);
            //int.TryParse(serialText, out serial);
            var fileId = obj.FileId;
            var serial = obj.Serial;

            if (fileId > 0)
            {
                using (var db = new SysContext())
                {
                    sys_file file = null;
                    if (serial > 0)
                    {
                        file = db.sys_file.Find(fileId, serial);
                    }
                    else
                    {
                        file = db.sys_file.FirstOrDefault(f => f.FileId == fileId);
                    }
                    if (file != null)
                    {
                        var path = Server.MapPath("~" + file.Path);
                        if (System.IO.File.Exists(path))
                        {
                            //return File(path, MimeMapping.GetMimeMapping(path), file.FileName);
                            return File(path, MimeMapping.GetMimeMapping(path));
                        }
                    }
                }
            }
            return HttpNotFound();
        }

    }
}