using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MongoDB.Bson;

namespace Evaluations.Models
{
    public class Courses
    {
        [BsonId]
        public ObjectId ObjectId;

        [ScaffoldColumn(false)]
        public String Id { get { return ObjectId.ToString(); } set { ObjectId = new ObjectId(value); } }
        public String CourseTitle { get; set; }
        public String ActivityTitle { get; set; }
        public String Supervisor { get; set; }
        public String Barcode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String CourseStatus { get; set; }
        public String SurveyStatus { get; set; }
        public int CourseId { get; set; }
        public int ActivityId { get; set; }
        public DateTime? ExpirationDate { get; set; }

    }
}