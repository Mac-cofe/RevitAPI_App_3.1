using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RevitAPITraining_lab3._1
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            TaskDialog.Show("Сообщение", "Выберите стены, объем которых требуется получить, после выбора нажмите Finish");

            IList<Reference> selecetedWallsRefList = uidoc.Selection
                .PickObjects(ObjectType.Element, new WallFilter(), "Выбор стен");  // применяем фильтр 

            double volume = 0; // начальное значение
            foreach (var selectedWallRef in selecetedWallsRefList)
            {
                Wall wall = doc.GetElement(selectedWallRef) as Wall;
                Parameter volumeParameter = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                if (volumeParameter.StorageType == StorageType.Double)
                {
                    volume += volumeParameter.AsDouble();
                }
            }

            double volumeMeters = UnitUtils.ConvertFromInternalUnits(volume, UnitTypeId.CubicMeters); // перевод единиц

            TaskDialog.Show("Объём выбранных стен", "Объём выделенных стен:"+volumeMeters.ToString());
            return Result.Succeeded;
        }
    }
}