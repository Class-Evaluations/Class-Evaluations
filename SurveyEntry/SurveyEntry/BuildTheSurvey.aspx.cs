using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;


namespace SurveyEntry
{
    public partial class BuildTheSurvey : System.Web.UI.Page
    {
        int courseID;
        int surveyID;
        int survey_request_sentID;
        int questionID;
        string answerName;
        int answerTypeID;
        string tablename;
        string userHash;
        int answerID;
        string answerText;
        int answerInt;
        int answerCount;
        string statusFlag;
        string SurveyHeader;
        string SurveyIndroduction;
        int NumOfSections;
        string programName;
        string facilityUsed;
        DateTime programDate;
        int surveySectionID;
        string sectionTitle;
        int sectionIdOrder;
        string surveyConn;
        string classConn;
        int questionId;
        string questionText;
        int answerTypeId;
        string answerTypeName;
        string controlName;
        string choiceText;
        string condition;
        int uniqueID = 0;
        int scale;
        int multi;
        int tlong;
        int true_false;
        string courseTitle;
        string activityTitle;


        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;
            //ListControlCollections();
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;

            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            System.IO.FileInfo pagename = new System.IO.FileInfo(sPath);
            userHash = oInfo.Name;
            //System.IO.Path redirectURL = System.Web.HttpContext.Current.Request.Url;

            //CHECK THE USER unique identifyier to see if the survey has already been answered
            surveyConn = ConfigurationManager.ConnectionStrings["Survey_DB"].ConnectionString;
            classConn = ConfigurationManager.ConnectionStrings["CLASS_DB"].ConnectionString;

            
            //get the unique identifier for the answer tables for this user.
            using (SqlConnection conn = new SqlConnection(surveyConn))
            using (SqlCommand cmd = new SqlCommand("SELECT survey_request_sent_id, course_id, survey_id, status_flag FROM SURVEY_REQUEST_SENT WHERE person_hash = '" + userHash + "'", conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    survey_request_sentID = reader.GetInt32(0);
                    courseID = reader.GetInt32(1);
                    surveyID = reader.GetInt32(2);
                    statusFlag = reader.GetString(3);
                }

                if (statusFlag == "X")
                {
                    Response.Redirect("~/SurveyExpired.aspx", true);

                }

            }

            using (SqlConnection conn = new SqlConnection(surveyConn))
            using (SqlCommand cmd = new SqlCommand("SELECT count(survey_request_sent_id) FROM ANSWER WHERE survey_request_sent_id = '" + survey_request_sentID + "'", conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    answerCount = reader.GetInt32(0);
                }
                else 
                {
                    Response.Redirect("~/Error.aspx", true);

                }

            }

            if (answerCount > 0)
            {
                Response.Redirect("~/AlreadyAnswered.aspx", true);
            }


            //Get the program information and populate the page fields

            using (SqlConnection conn = new SqlConnection(classConn))
            using (SqlCommand cmdtitle = new SqlCommand("SELECT f.facility_name, c.booking_start_date, c.title, a.title FROM CLASS.dbo.REGISTRATION r " +
                                                        "JOIN CLASS.dbo.COURSE c ON c.course_id = r.course_id JOIN CLASS.dbo.ACTIVITY a ON a.activity_id = c.activity_id " +
                                                        "JOIN CLASS.dbo.FACILITY f ON f.facility_id = first_facility WHERE c.course_id" + courseID, conn))
            {
                conn.Open();
                SqlDataReader reader = cmdtitle.ExecuteReader();

                if (reader.Read())
                {
                    facilityUsed = reader.GetString(0);
                    programDate = reader.GetDateTime(1);
                    if (!String.IsNullOrEmpty(reader.GetString(2)))
                    {
                        courseTitle = reader.GetString(2);
                    }
                    else
                    {
                        courseTitle = " ";
                    }
                    if (!String.IsNullOrEmpty(reader.GetString(3)))
                    {
                        activityTitle = reader.GetString(3);
                    }
                    else 
                    {
                        activityTitle = " ";
                    }
                }

                programName = (courseTitle).Trim() + (activityTitle).Trim();


                txtprogramName.Text = programName;
                txtfacilityUsed.Text = facilityUsed;
                txtprogramDates.Text = Convert.ToString(programDate);
            }


            //Build the survey based on the items returned from the Survey_DB tables
            using (SqlConnection conn = new SqlConnection(surveyConn))
            using (SqlCommand cmdTitle = new SqlCommand("Select survey_id, title, header_text, survey_introduction, number_of_sections From SURVEY WHERE survey_id = " + surveyID, conn))
            {
                conn.Open();
                SqlDataReader reader = cmdTitle.ExecuteReader();

                if (reader.Read())
                {
                    Title = reader.GetString(1);
                    SurveyHeader = reader.GetString(2);
                    SurveyIndroduction = reader.GetString(3);
                    NumOfSections = reader.GetInt32(4);

                }

                LabelTitle.Text = SurveyHeader;
                LabelIntroduction.Text = SurveyIndroduction;




            }

            //get the questions per section and create the controls
            using (SqlConnection conn = new SqlConnection(surveyConn))
            using (SqlCommand cmdSectionInfo = new SqlCommand("Select  ss.survey_section_id, ss.title, ss.section_id_order, sq.survey_id, " +
                                                               "q.question_id, q.question_text, at.answer_type_id, at.answer_type_name, at.controlName " +
                                                               "from SURVEY_SECTION ss inner join dbo.SURVEY_QUESTIONS sq ON sq.section_id = ss.survey_section_id " +
                                                               "inner join QUESTION q ON q.question_id = sq.question_id inner join ANSWER_TYPE at ON at.answer_type_id = q.answer_type_id " +
                                                               "where sq.survey_id = " + surveyID + " order by section_id_order", conn))
            {


                conn.Open();
                SqlDataReader reader = cmdSectionInfo.ExecuteReader();
                int sectionID = 0;
                int counter = 1;


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        surveySectionID = reader.GetInt32(0);
                        sectionTitle = reader.GetString(1);
                        sectionIdOrder = reader.GetInt32(2);
                        surveyID = reader.GetInt32(3);
                        questionId = reader.GetInt32(4);
                        questionText = reader.GetString(5);
                        answerTypeId = reader.GetInt32(6);
                        answerTypeName = reader.GetString(7);
                        controlName = reader.GetString(8);


                        uniqueID++;
                        if (surveySectionID != sectionID)
                        {
                            Label label1 = new Label();
                            label1.Text = sectionTitle;
                            label1.ID = "Label" + counter;
                            panelContent.Controls.Add(new LiteralControl("<strong>"));
                            panelContent.Controls.Add(label1);
                            panelContent.Controls.Add(new LiteralControl("</strong >"));
                            panelContent.Controls.Add(new LiteralControl("<br /> <br />"));
                            sectionID = surveySectionID;
                        }

                        Label LabelName = new Label();
                        LabelName.ID = "LabelName" + uniqueID;
                        LabelName.Text = counter + ". ";
                        counter++;
                        uniqueID++;

                        Label LabelName2 = new Label();
                        LabelName2.ID = "LabelName" + uniqueID;
                        LabelName2.Text = questionText;

                        panelContent.Controls.Add(LabelName);
                        panelContent.Controls.Add(LabelName2);
                        panelContent.Controls.Add(new LiteralControl("<br /> <br />"));
                        uniqueID++;

                        //Scale                                   
                        //Multit-Choice                           
                        //Mulit-Choice-multi                      
                        //Answer_long                            
                        //Answer_short                            
                        //True_False 
                        //determine what answer type

                        switch (answerTypeName.Trim())
                        {
                            case "Scale":
                                scale = answerTypeId;
                                RadioButtonList RadioButtonList1 = new RadioButtonList();
                                RadioButtonList1.ID = Convert.ToString(questionId);
                                RadioButtonList1.AutoPostBack = true;
                                RadioButtonList1.Items.Add(new ListItem("Strongly Agree", "5"));
                                RadioButtonList1.Items.Add(new ListItem("Agree", "4"));
                                RadioButtonList1.Items.Add(new ListItem("Neutral", "3"));
                                RadioButtonList1.Items.Add(new ListItem("Disagree", "2"));
                                RadioButtonList1.Items.Add(new ListItem("Strongly Disagree", "1"));
                                RadioButtonList1.Items.Add(new ListItem("NA", "-1"));

                                panelContent.Controls.Add(RadioButtonList1);
                                panelContent.Controls.Add(new LiteralControl("<br /> <br /> <br />"));
                                break;
                            case "Multi-Choice":
                                scale = answerTypeId;
                                RadioButtonList RadioButtonList2 = new RadioButtonList();
                                RadioButtonList2.ID = Convert.ToString(questionId);
                                RadioButtonList2.AutoPostBack = true;

                                //Get the choice values.
                                using (SqlConnection conn1 = new SqlConnection(surveyConn))
                                using (SqlCommand choiceValues = new SqlCommand("Select choice_text from QUESTION_MULTIPLE_CHOICE Where question_id = " + questionId, conn1))
                                {
                                    conn1.Open();
                                    SqlDataReader reader1 = choiceValues.ExecuteReader();

                                    if (reader1.HasRows)
                                    {
                                        while (reader1.Read())
                                        {
                                            choiceText = reader1.GetString(0);
                                            RadioButtonList2.Items.Add(choiceText);
                                        }
                                    }
                                }
                                panelContent.Controls.Add(RadioButtonList2);
                                panelContent.Controls.Add(new LiteralControl("<br /> <br /> <br />"));
                                break;
                            case "Multi-Choice-multi":
                                multi = answerTypeId;
                                CheckBoxList CheckBoxList1 = new CheckBoxList();
                                CheckBoxList1.ID = Convert.ToString(questionId);
                                //CheckBoxList1.SelectionMode = multiple;
                                CheckBoxList1.AutoPostBack = true;

                                //Get the choice values.
                                using (SqlConnection conn2 = new SqlConnection(surveyConn))
                                using (SqlCommand choiceValues = new SqlCommand("Select choice_text from QUESTION_MULTIPLE_CHOICE Where question_id = " + questionId, conn2))
                                {
                                    conn2.Open();
                                    SqlDataReader reader2 = choiceValues.ExecuteReader();

                                    if (reader2.HasRows)
                                    {
                                        while (reader2.Read())
                                        {
                                            choiceText = reader2.GetString(0);
                                            CheckBoxList1.Items.Add(choiceText);
                                        }
                                    }
                                }
                                panelContent.Controls.Add(CheckBoxList1);
                                panelContent.Controls.Add(new LiteralControl("<br /> <br /> <br />"));
                                break;
                            case "Answer_short":
                                TextBox TextBox1 = new TextBox();
                                TextBox1.ID = Convert.ToString(questionId);
                                TextBox1.AutoPostBack = true;
                                TextBox1.Style["BorderStyle"] = "Solid";
                                TextBox1.Style["Height"] = "61px";
                                TextBox1.Style["Width"] = "620px";
                                panelContent.Controls.Add(TextBox1);
                                panelContent.Controls.Add(new LiteralControl("<br /> <br /> <br />"));
                                break;
                            case "Answer_long":
                                tlong = answerTypeId;
                                TextBox TextBox2 = new TextBox();
                                TextBox2.ID = Convert.ToString(questionId);
                                TextBox2.AutoPostBack = true;
                                TextBox2.Style["BorderStyle"] = "Solid";
                                TextBox2.Style["Height"] = "61px";
                                TextBox2.Style["Width"] = "620px";
                                panelContent.Controls.Add(TextBox2);
                                panelContent.Controls.Add(new LiteralControl("<br /> <br /> <br />"));
                                break;
                            case "True_False":
                                true_false = answerTypeId;
                                RadioButtonList RadioButtonList3 = new RadioButtonList();
                                RadioButtonList3.AutoPostBack = true;
                                RadioButtonList3.ID = Convert.ToString(questionId);
                                RadioButtonList3.Items.Insert(0, new ListItem("False", "0"));
                                RadioButtonList3.Items.Insert(1, new ListItem("True", "1"));

                                panelContent.Controls.Add(RadioButtonList3);
                                panelContent.Controls.Add(new LiteralControl("<br /> <br /> <br />"));
                                break;
                            default:
                                break;
                        }

                    }

                }

            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            //get the unique identifier for the answer tables for this user.
            using (SqlConnection conn = new SqlConnection(surveyConn))
            using (SqlCommand cmd = new SqlCommand("SELECT survey_request_sent_id, course_id, survey_id FROM SURVEY_REQUEST_SENT WHERE person_hash = '" + userHash + "'", conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    survey_request_sentID = reader.GetInt32(0);
                    courseID = reader.GetInt32(1);
                    surveyID = reader.GetInt32(2);
                }

            }

            foreach (Control ctrl in this.Form.Controls)
            {
                if (ctrl is Panel)
                {
                    Panel chp = ((System.Web.UI.WebControls.Panel)ctrl);
                    foreach (Control ctrl2 in chp.Controls)
                    {

                        if (ctrl2 is TextBox)
                        {
                            questionID = Convert.ToInt32(ctrl2.ID);
                            TextBox answer = ((System.Web.UI.WebControls.TextBox)ctrl2);
                            answerText = answer.Text;

                            answerTypeID = GetAnswerType();

                            if (!String.IsNullOrEmpty(answerText))
                            { answerText = (answerText).Replace("'", "''"); }
                            //string aname = answerTypeName;
                            InsertAnswer();
                        }

                        //Need to loop therough the checkboxes and concatenate the multiple selected answers.
                        //currently is inserted the last answer.

                        if (ctrl2 is CheckBoxList)
                        {
                            questionID = Convert.ToInt32(ctrl2.ID);

                            answerTypeID = GetAnswerType();

                            CheckBoxList answer = ((System.Web.UI.WebControls.CheckBoxList)ctrl2);
                            answerText = answer.SelectedValue;

                            for (int i = 0; i < answer.Items.Count; i++)
                            {
                                if (answer.Items[i].Selected)
                                {
                                    if (!String.IsNullOrEmpty(answer.SelectedValue))
                                    {
                                        //answerText = answer.SelectedValue;
                                        answerText = answer.Items[i].Text;
                                        answerText = (answerText).Replace("'", "''");
                                        //answerTypeID = 4;
                                        InsertAnswer();
                                    }

                                }
                            }

                        }

                        if (ctrl2 is RadioButtonList)
                        {
                            questionID = Convert.ToInt32(ctrl2.ID);
                            RadioButtonList answer = ((System.Web.UI.WebControls.RadioButtonList)ctrl2);

                            answerTypeID = GetAnswerType();

                            switch (answerTypeID)
                            {
                                case 2:
                                    answerInt = Convert.ToInt32(answer.SelectedIndex);
                                    break;
                                case 3:
                                    answerText = answer.SelectedValue;
                                    break;
                                //case 7:
                                //    answerInt = answerInt = Convert.ToInt32(answer.SelectedIndex);
                                //    break;
                                default:
                                    break;
                            }
                            //answerTypeID = scale;
                            InsertAnswer();
                        }
                    }
                }
            }
            Response.Redirect("~/ThankYou.aspx", true);
        }


        protected void InsertAnswer()
        {
          //INSERT INTO THE ANSWER TABLE THEN RETRIEVE THE ANSWER ID
            using (SqlConnection conn = new SqlConnection(surveyConn))
            using (SqlCommand cmd4 = new SqlCommand("INSERT INTO ANSWER (question_id, answer_type_id, survey_request_sent_id) VALUES (" + questionID + ", " + answerTypeID + ", " + survey_request_sentID + ")", conn))
            {
                conn.Open();
                SqlDataReader reader = cmd4.ExecuteReader();
            }


            //GET THE ANSWER TYPE NAME TO DETERMINE THE TABLE TO INSERT ANSWER AND GET THE ANSWER ID FOR ALL INSERTS
            using (SqlConnection conn = new SqlConnection(surveyConn))
            using (SqlCommand cmd5 = new SqlCommand("SELECT answer_id, question_id FROM ANSWER WHERE question_id = " + questionID, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd5.ExecuteReader();

                if (reader.Read())
                {
                    answerID = reader.GetInt32(0);
                }
            }

            // go get answer type to determine what answer table to insert the answer

            using (SqlConnection conn = new SqlConnection(surveyConn))
            using (SqlCommand cmd6 = new SqlCommand("SELECT answer_type_name FROM ANSWER_TYPE WHERE answer_type_id = " + answerTypeID, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd6.ExecuteReader();

                if (reader.Read())
                {
                    answerName = reader.GetString(0);
                }

            }


            switch (answerName.Trim())
            {
                case "Scale":
                    tablename = "ANSWER_SCALE";
                    condition = "(" + answerID + ", " + answerInt + ", " + survey_request_sentID + ")";
                    break;
                case "Multi-Choice":
                    tablename = "ANSWER_MULTIPLE_CHOICE";
                    condition = "(" + answerID + ", '" + answerText + "', " + survey_request_sentID + ")";
                    break;
                case "Multi-Choice-multi":
                    tablename = "ANSWER_MULTIPLE_CHOICE";
                    condition = "(" + answerID + ", '" + answerText + "', " + survey_request_sentID + ")";
                    break;
                case "Answer_short":
                    tablename = "ANSWER_SHORT";
                    condition = "(" + answerID + ", '" + answerText + "', " + survey_request_sentID + ")";
                    break;
                case "Answer_long":
                    tablename = "ANSWER_LONG";
                    condition = "(" + answerID + ", '" + answerText + "', " + survey_request_sentID + ")";
                    break;
                case "True_False":
                    tablename = "ANSWER_TRUE_FALSE";
                    condition = "(" + answerID + ", " + answerInt + ", " + survey_request_sentID + ")";
                    break;
                default:
                    break;
            }



            using (SqlConnection conn = new SqlConnection(surveyConn))
            using (SqlCommand cmd6 = new SqlCommand("INSERT INTO " + tablename + " (answer_id, submitted_answer, survey_request_sent_id) VALUES " + condition, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd6.ExecuteReader();
            }

        }

        public int GetAnswerType()
        {
            //add a query to get the answer type.   
            string dsn = "Data Source=10.6.5.69;Initial Catalog=Survey_DB;User Id=surveyhelper; Password=helpme";
            using (SqlConnection conn = new SqlConnection(surveyConn))
            using (SqlCommand cmd = new SqlCommand("SELECT answer_type_id FROM QUESTION WHERE question_id = " + questionID, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    answerTypeID = reader.GetInt32(0);
                }
            }
            return answerTypeID;
        }
    }
}
