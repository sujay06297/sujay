using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClass.Models;

namespace WebClass.Service
{
    public class LessonService
    {
        AOnlineClassEntities db = new AOnlineClassEntities();

        public List<C_Lesson> LessonList(int id)
        {

            return db.C_Lesson.Where(c => c.ClassID == id).ToList();
        }


    }
}