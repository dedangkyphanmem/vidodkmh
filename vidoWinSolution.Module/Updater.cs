using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

namespace vidoWinSolution.Module {
    public class Updater : ModuleUpdater {
        public Updater(Session session, Version currentDBVersion) : base(session, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            DomainObject1 do1 = Session.FindObject<DomainObject1>(new BinaryOperator("Property1", "StartupObject"));
            if (do1 == null) {
                do1 = new DomainObject1(Session);
                do1.Property1 = "StartupObject";
                do1.Save();
            }
        }
    }
}
