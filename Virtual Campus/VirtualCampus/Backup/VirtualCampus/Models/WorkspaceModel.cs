using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualCampus.Models
{
    public class WorkspaceModel {
        public int Workspace_id { set; get; }
        public int Owner_Id { get; set; }
        public string Workspace_Name { get; set; }
    }
    public class WorkspaceUserModel {
        public int WorkspaceUser_Id { get; set; }
        public int Workspace_Id { get; set; }
        public int User_Id { get; set; }
    }
    public class WorkspaceTask {
        public int Task_id { get; set; }
        public int WorkspaceUser_Id { get; set; }
        public int Workspace_Id { set; get; }
        public int Faculty_Id { get; set; }
        public string Task_name { get; set; }
        public DateTime Task_Deadline { set; get; }
    }
    public class Application {
        public int Application_Id { get; set; }
        public int Workspace_Id { get; set; }
        public int WorkspaceUser_Id { get; set; }
        public string Application_Name { get; set; }
        public string DateLabel_1 { get; set; }
        public string TextLabel_1 { get; set; }
        public string TextLabel_2 { get; set; }
        public string TextLabel_3 { get; set; }
        public string TextLabel_4 { get; set; }
        public string TextLabel_5 { get; set; }
        public string TextArea_1 { get; set; }
        public string TextArea_2 { get; set; }
        public string TextArea_3 { get; set; }
    }
}