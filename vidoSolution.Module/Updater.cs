using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using vidoSolution.Module.DomainObject;
using DevExpress.ExpressApp.Reports;
using DevExpress.ExpressApp;
using System.Drawing;

namespace vidoSolution.Module
{
    public class Updater : ModuleUpdater
    {
        public Updater(Session session, Version currentDBVersion) : base(session, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema()
        {

            base.UpdateDatabaseAfterUpdateSchema();
            NestedUnitOfWork UOW = this.Session.BeginNestedUnitOfWork();
            UOW.BeginTransaction();
            //khoi tao quan tai khoan quan tri he thong (mau)
            User systemAmdmin = UOW.FindObject<User>(new BinaryOperator("UserName", "SystemAdmin"));
            if (systemAmdmin == null)
            {
                systemAmdmin = new User(UOW);
                systemAmdmin.UserName = "SystemAdmin";
                systemAmdmin.SetPassword("SystemAdmin");
            }
            //khoi tao quan tai khoan sinh vien (mau)
            Student student = UOW.FindObject<Student>(new BinaryOperator("UserName", "SampleStudent"));
            if (student == null)
            {
                student = new Student(UOW);
                student.UserName = "SampleStudent";
                student.SetPassword("SampleStudent");
            }
            User guest = UOW.FindObject<User>(new BinaryOperator("UserName", "guest"));
            if (guest == null)
            {
                guest = new User(UOW);
                guest.UserName = "guest";
                guest.SetPassword("guest");
            }
            //khoi tao quan tai khoan quan quan ly dao tao
            User dataAdmin = UOW.FindObject<User>(new BinaryOperator("UserName", "DataAdmin"));
            if (dataAdmin == null)
            {
                dataAdmin = new User(UOW);
                dataAdmin.UserName = "DataAdmin";
                dataAdmin.SetPassword("DataAdmin");
            }

                     
           
            // If a role with the Administrators name does not exist in the database, create this role 
            Role adminRole = UOW.FindObject<Role>(new BinaryOperator("Name", "Administrators"));
            if (adminRole == null)
            {
                adminRole = new Role(UOW);
                adminRole.Name = "Administrators";
            }
            // If a role with the Users name does not exist in the database, create this role 
            Role userRole = UOW.FindObject<Role>(new BinaryOperator("Name", "Users"));
            if (userRole == null)
            {
                userRole = new Role(UOW);
                userRole.Name = "Users";
            }

            // If a role with the Users name does not exist in the database, create this role 
            Role dataRole = UOW.FindObject<Role>(new BinaryOperator("Name", "DataAdmins"));
            if (dataRole == null)
            {
                dataRole = new Role(UOW);
                dataRole.Name = "DataAdmins";
            }

            Role studentRole = UOW.FindObject<Role>(new BinaryOperator("Name", "Students"));
            if (studentRole == null)
            {
                studentRole = new Role(UOW);
                studentRole.Name = "Students";
            }
           
             //Delete all permissions assigned to the Administrators and Users roles 

            //while (adminRole.PersistentPermissions.Count > 0)
            //{
            //    UOW.Delete(adminRole.PersistentPermissions[0]);
            //}
            //while (userRole.PersistentPermissions.Count > 0)
            //{
            //    UOW.Delete(userRole.PersistentPermissions[0]);
            //}
            //while (dataRole.PersistentPermissions.Count > 0)
            //{
            //    UOW.Delete(dataRole.PersistentPermissions[0]);
            //}
            //while (studentRole.PersistentPermissions.Count > 0)
            //{
            //    UOW.Delete(studentRole.PersistentPermissions[0]);
            //}
            //UOW.PurgeDeletedObjects();
            

            // Allow full access to all objects to the Administrators role 
            adminRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.AllAccess));
            // Allow editing the Application Model to the Administrators role 
            adminRole.AddPermission(new EditModelPermission(ModelAccessModifier.Allow));
            // Save the Administrators role to the database 
            adminRole.Save();

            // Allow full access to all objects to the DataAdmins role 
            dataRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.AllAccess));
            // Allow editing the Application Model to the Administrators role 
            dataRole.AddPermission(new EditModelPermission(ModelAccessModifier.Allow));
            // Save the Administrators role to the database 
            dataRole.Save();

            AddRoleToUser(userRole);
            // Allow full access to all objects to the Users role 
           
            userRole.Save();
            AddRoleToStudent(studentRole);
            studentRole.Save();
            // Add the Administrators role to the user1 
            systemAmdmin.Roles.Add(adminRole);
            dataAdmin.Roles.Add(dataRole);
            student.Roles.Add(studentRole);
            guest.Roles.Add(userRole);
            systemAmdmin.Save();
            dataAdmin.Save();
            student.Save();
            guest.Save();

            UOW.CommitTransaction();
           
            //First Mynote
            MyNote mynote = this.Session.FindObject<MyNote>(new BinaryOperator("Title", "Welcome"));
            if ((mynote == null))
            {
                mynote = new MyNote(Session);
                mynote.Title = "Welcome";
                mynote.Intro = "Welcome all to visit our website!!! ";
                mynote.FullContent = "Change full content here!!! ";
                mynote.Save();
            }
            
            //create report
            CreateReport("Bảng điểm nhóm MH");
            CreateReport("Bảng điểm nhóm MH (theo nhóm lớp)");
            CreateReport("Danh sách lớp biên chế");
            CreateReport("Danh sách sinh viên theo lớp");
            CreateReport("Điểm học kỳ");
            CreateReport("Điểm tích lũy");
            CreateReport("DTBHKReport");
            CreateReport("Giao dịch học phí sinh viên");
            CreateReport("Kết quả ĐK 1 SV");
            CreateReport("Kết quả ĐK tất cả SV");            
            CreateReport("Kết quả học tập");           
            CreateReport("Lịch giảng viên");
            CreateReport("Lịch lớp biên chế");
            CreateReport("Lịch môn học");            
            CreateReport("Lịch phòng học cơ sở");
            CreateReport("Lịch phòng học");
            CreateReport("Lịch sinh viên");
            CreateReport("Tỉ lệ nợ lớp biên chế NHHK");
            CreateReport("Tỉ lệ nợ sinh viên lớp biên chế");
            CreateParam();

        }

        private void AddRoleToUser(Role userRole)
        {
            
            //userRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.AllAccess, ObjectAccessModifier.Deny));
            userRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.AllAccess));
            userRole.AddPermission(new ObjectAccessPermission(typeof(IXPSimpleObject), ObjectAccess.NoAccess));

            userRole.AddPermission(new ObjectAccessPermission(typeof(MyNote), ObjectAccess.AllAccess, ObjectAccessModifier.Allow));
            userRole.AddPermission(new ObjectAccessPermission(typeof(MyNote), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            userRole.AddPermission(new ObjectAccessPermission(typeof(MyHelp), ObjectAccess.AllAccess, ObjectAccessModifier.Allow));
            userRole.AddPermission(new ObjectAccessPermission(typeof(MyHelp), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            userRole.AddPermission(new ObjectAccessPermission(typeof(PopUpMessage), ObjectAccess.AllAccess, ObjectAccessModifier.Allow));
            userRole.AddPermission(new ObjectAccessPermission(typeof(PopUpMessage), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            
            userRole.AddPermission(new ObjectAccessPermission(typeof(User), ObjectAccess.AllAccess));
            userRole.AddPermission(new ObjectAccessPermission(typeof(User), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));

            //userRole.AddPermission(new ObjectAccessPermission(typeof(Student), ObjectAccess.AllAccess));
            //userRole.AddPermission(new ObjectAccessPermission(typeof(Student), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            userRole.AddPermission(new EditModelPermission(ModelAccessModifier.Deny));
        }

        private void AddRoleToStudent(Role studentRole)
        {
            studentRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(IXPSimpleObject), ObjectAccess.NoAccess));

            studentRole.AddPermission(new ObjectAccessPermission(typeof(User), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(User), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Role), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Role), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Student), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Student), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(RegisterDetail), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(RegisterDetail), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(PopUpMessage), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(PopUpMessage), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));

            studentRole.AddPermission(new ObjectAccessPermission(typeof(StudentAccumulation), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(StudentAccumulation), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(StudentResult), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(StudentResult), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(AccountTransaction), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(AccountTransaction), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));

            studentRole.AddPermission(new ObjectAccessPermission(typeof(StudentClass), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(StudentClass), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Subject), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Subject), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Branch), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Branch), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Semester), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Semester), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Department), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Department), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Lesson), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Lesson), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Teacher), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Teacher), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(TkbLesson), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(TkbLesson), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(TkbSemester), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(TkbSemester), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Classroom), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Classroom), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Office), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(Office), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(RegisterState), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(RegisterState), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(RegisterTime), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(RegisterTime), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(MyNote), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(MyNote), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(MyHelp), ObjectAccess.AllAccess));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(MyHelp), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(WeekReportData), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(WeekReportData), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(AccountTransactionTracking), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            studentRole.AddPermission(new ObjectAccessPermission(typeof(AccountTransactionTracking), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(ClassTransactionTracking), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(ClassTransactionTracking), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(AccountTransactionFile), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(AccountTransactionFile), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(StudentFile), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(StudentFile), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(TeacherFile), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(TeacherFile), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(SubjectFile), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(SubjectFile), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(StudentResultFile), ObjectAccess.AllAccess ^ ObjectAccess.Navigate));
            //studentRole.AddPermission(new ObjectAccessPermission(typeof(StudentResultFile), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
                      

            studentRole.AddPermission(new EditModelPermission(ModelAccessModifier.Deny));
        }
        private void CreateReport(string reportName)
        {
            
            ReportData reportdata = Session.FindObject<ReportData>(
               new BinaryOperator("Name", reportName));
            if (reportdata == null)
            {
                reportdata = new ReportData(Session);
                XafReport rep = new XafReport();
                rep.LoadLayout(GetType().Assembly.GetManifestResourceStream(
                   "vidoSolution.Module.EmbeddedReports." + reportName + ".repx"));
                rep.ReportName = reportName;
                reportdata.SaveXtraReport(rep);
                reportdata.Save();
            }
            else
            {
                XafReport rep = new XafReport();
                rep.LoadLayout(GetType().Assembly.GetManifestResourceStream(
                   "vidoSolution.Module.EmbeddedReports." + reportName + ".repx"));
                reportdata.SaveXtraReport(rep);                
                reportdata.Save();
            }
        }
        private void CreateParam()
        {
            ConstrainstParameter cp = Session.FindObject<ConstrainstParameter>(
                new BinaryOperator("Code", "REGISTERSEMESTER"));
            if (cp == null)
            {
                cp = new ConstrainstParameter(Session);
                cp.Code = "REGISTERSEMESTER";
                cp.Name = "NHHK hiện tại";
                cp.Value = 0;
                cp.Save();
            }
            cp = Session.FindObject<ConstrainstParameter>(
                new BinaryOperator("Code", "MINCREDITS"));
            if (cp == null)
            {
                cp = new ConstrainstParameter(Session);
                cp.Code = "MINCREDITS";
                cp.Name = "Số tín chỉ Min";
                cp.Value = 15;
                cp.Save();
            }
            cp = Session.FindObject<ConstrainstParameter>(
                new BinaryOperator("Code", "MAXCREDITS"));
            if (cp == null)
            {
                cp = new ConstrainstParameter(Session);
                cp.Code = "MAXCREDITS";
                cp.Name = "Số tín chỉ Max";
                cp.Value = 25;
                cp.Save();
            }
            cp = Session.FindObject<ConstrainstParameter>(
                new BinaryOperator("Code", "TUITIONFEEPERCREDIT"));
            if (cp == null)
            {
                cp = new ConstrainstParameter(Session);
                cp.Code = "TUITIONFEEPERCREDIT";
                cp.Name = "Số tiền học phí cho 01 tín chỉ (VND)";
                cp.Value = 150000;
                cp.Save();
            }
            cp = Session.FindObject<ConstrainstParameter>(
                new BinaryOperator("Code", "MINDEPTCANENROLL"));
            if (cp == null)
            {
                cp = new ConstrainstParameter(Session);
                cp.Code = "MINDEPTCANENROLL";
                cp.Name = "Số tiền nợ học phí tối thiểu có thể đăng ký môn học (VNĐ)- (Sinh viên không được nợ nhiều hơn)";
                cp.Value = 199999;
                cp.Save();
            }
            cp = Session.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MINDEPTCANPRINTRESULT"));
            if (cp == null)
            {
                cp = new ConstrainstParameter(Session);
                cp.Code = "MINDEPTCANPRINTRESULT";
                cp.Name = "Số tiền nợ học phí tối thiểu có thể in được bảng điểm (VNĐ)- (Sinh viên không được nợ nhiều hơn)";
                cp.Value = 499999;
                cp.Save();
            }

            cp = Session.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "MORNINGPERIODCOUNT"));
            if (cp == null)
            {
                cp = new ConstrainstParameter(Session);
                cp.Code = "MORNINGPERIODCOUNT";
                cp.Name = "Số tiết buổi sáng";
                cp.Value = 6;
                cp.Save();
            }

            cp = Session.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "AFTERNOONPERIODCOUNT"));
            if (cp == null)
            {
                cp = new ConstrainstParameter(Session);
                cp.Code = "AFTERNOONPERIODCOUNT";
                cp.Name = "Số tiết buổi chiều";
                cp.Value = 6;
                cp.Save();
            }
            cp = Session.FindObject<ConstrainstParameter>(
               new BinaryOperator("Code", "EVERNINGPERIODCOUNT"));
            if (cp == null)
            {
                cp = new ConstrainstParameter(Session);
                cp.Code = "EVERNINGPERIODCOUNT";
                cp.Name = "Số tiết buổi tối";
                cp.Value = 3;
                cp.Save();
            }


            RegisterState regstate= Session.FindObject<RegisterState>(
                new BinaryOperator("Code", "SELECTED"));
            if (regstate == null)
            {
                regstate = new RegisterState(Session);
                regstate.Code = "SELECTED";
                regstate.Name = "Đã chọn";
                regstate.Type= "DKMH";
                regstate.Save();
            }
            regstate = Session.FindObject<RegisterState>(
                new BinaryOperator("Code", "BOOKED"));
            if (regstate == null)
            {
                regstate = new RegisterState(Session);
                regstate.Code = "BOOKED";
                regstate.Name = "Đã đăng ký";
                regstate.Type = "DKMH";
                regstate.Save();
            }
            
            regstate = Session.FindObject<RegisterState>(
              new BinaryOperator("Code", "PRINTED"));
            if (regstate == null)
            {
                regstate = new RegisterState(Session);
                regstate.Code = "PRINTED";
                regstate.Name = "Đã in biên lai";
                regstate.Type = "DKMH";
                regstate.Save();
            }
            regstate = Session.FindObject<RegisterState>(
              new BinaryOperator("Code", "CANCELED"));
            if (regstate == null)
            {
                regstate = new RegisterState(Session);
                regstate.Code = "CANCELED";
                regstate.Name = "Đã hủy";
                regstate.Type = "KIEMTRA";
                regstate.Save();
            }
            regstate = Session.FindObject<RegisterState>(
              new BinaryOperator("Code", "INSERTED"));
            if (regstate == null)
            {
                regstate = new RegisterState(Session);
                regstate.Code = "INSERTED";
                regstate.Name = "Đã thêm";
                regstate.Type = "KIEMTRA";
                regstate.Save();
            }
            regstate = Session.FindObject<RegisterState>(
                new BinaryOperator("Code", "CHECKED"));
            if (regstate == null)
            {
                regstate = new RegisterState(Session);
                regstate.Code = "CHECKED";
                regstate.Name = "Đã kiểm tra";
                regstate.Type = "KIEMTRA";
                regstate.Save();
            }
            regstate = Session.FindObject<RegisterState>(
                new BinaryOperator("Code", "CONFIRMED"));
            if (regstate == null)
            {
                regstate = new RegisterState(Session);
                regstate.Code = "CONFIRMED";
                regstate.Name = "Đã xác nhận";
                regstate.Type = "KIEMTRA";
                regstate.Save();
            }
            regstate = Session.FindObject<RegisterState>(
               new BinaryOperator("Code", "NOTCHECKED"));
            if (regstate == null)
            {
                regstate = new RegisterState(Session);
                regstate.Code = "NOTCHECKED";
                regstate.Name = "Chưa kiểm tra";
                regstate.Type = "KIEMTRA";
                regstate.Save();
            }

        }

    }
}
