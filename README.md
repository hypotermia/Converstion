# DXMvc2DevExtreme

Test project is a test assignement to see how much time it takes to convert DevExpress MVC Extensions to DevExtreme MVC.

## Prerequisites
- VisualStudio 2022 Any version (not Code) With latest updates applied
- DevExpress ASP.NET Product v24.1.6 Locally installed (Trial version is ok)

## Quick Description of the project
The project is a simple ASP.NET MVC 5 project with DevExpress MVC Extensions.

The DataAccess layer is running on XPO / MS-Access database with Mapster for DTO and FluentValidator for Validation logic..
Accessing data is done by a DataStore. This is a generic base class which deals with DTO objects on the front and XPO Peristent objects internally.
Mapster is used inside the store to transform Queries and CRUD operations based on DTO Models to XPO Models including Validation.

In this project, there are 2 Stores; 
- PatientStore
- AppointmentStore
 
**This portion should not be changed during the conversion!!**

The home controller has a simple grid with a few columns and a few buttons to add, edit and delete patient records.
The AppointmentController hosts an ASPxScheduler with a few appointments linked to patients.

Both controllers descend from a Generic BaseController which has a few helper methods to deal with the stores.

## Conversion strategy and objectives 
In the Views folder, there are 2 subfolders; Home and Appointment.
The partialviews are now using DevExpress MVC Extensions which are server-side rendered and not compatible with .NET v8.
These cshtml files should be converted to use DevExtreme MVC components. 

**The functionality of the app should remain the same**

In other words:

`@Html.DevExpress().GridView(...)` should be replaced with `@Html.DevExtreme().DataGrid(...)`
`@Html.DevExpress().Scheduler(...)` should be replaced with `@Html.DevExtreme().Scheduler(...)`

It will be nessecary to add / modify controller endpoints for this to work.

## How to proceed:
1. Make sure you have DevExpress ASP.NET Product v24.1.6 installed
2. Clone this repository
3. Open the solution in Visual Studio 2022
4. Run the project and see how it works
5. Start converting the views in the Home and Appointment folders to use DevExtreme MVC components
6. Make sure the functionality remains the same
7. **Commit your changes to your own repository**
8. Send us the link to your repository


## Tips:
If you don't have a DevExpress license, you can use the trial version. (https://www.devexpress.com/products/try/)
