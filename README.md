Image Gallery
5 Azure Services Sample

Azure Services
- App Service - Web App
- Functions
- Azure Monitoring - Application Insights
- SQLDb
- Storage

Technologies
- ASP.NET Core 2.2
- Entity Framework
- Azure

Getting Started
1) Clone the repo
git clone https://github.com/Andrew-MSFT/BasicImageGallery.git 

2) create all Azure resources using the azuredeploy.json ARM template and parameters.  Optionally configure the azuredeploy.paramemters.json file. 

VS: 
- Open the ./CoreImageGallery/Deploy/CoreImageGalleryARM.sln
- Right-click the project, Deploy, New Deployment
- Choose your subscription
- Choose a new or existing Resource Group
- Click Deploy
- in the PowerShell prompt, set an admin password for the SQL database

Azure CLI:
- ls ./CoreImageGallery/Deploy/CoreImageGalleryARM/
- chmod +x ./deploy-azureresourcegroupbashcli.sh
- ./deploy-azureresourcegroupbashcli.sh

Azure PowerShell:
- cd ./CoreImageGallery/Deploy/CoreImageGalleryARM/
- Deploy-AzureResourceGroup.ps1

3) Publish the application code

VS:
- Open ./CoreImageGallery/CoreImageGallery.sln
- Right click the CoreImageGallery front-end Web project, Publish
- Select Existing in App Service menu
- Choose the subscription and Resource Group you justcreated in step #2, and then select the App Service Web App resource, e.g. 'coreimagegallerywebapp-nszlqnnhrb3le' (your ending will vary), and click OK
- Click Configure .. and go to the Settings tab (please wait as the Database configuration loads)
- Expand the Entity Framework Migrations settings under Database and check 'Apply migrations on publish' (this will enable ASP.NET users and roles using an initialized membership database)
- Save
- Publish
(your site will open up now.  In a few moments all service layers and the database will be warmed up and ready.  

4) Test the application

- Register a user by clicking the registration button.  Your user will be signed in.
- Upload one or more image files
- In a few moments refresh and you will see watermarked images that were processed by the function.  

