storage account to tigger events

`az storage account create -g az204 -n az204storageact  --allow-blob-public-access true -l eastus`

 - used portal to create a container called "container"
 - used VS to generate project.  it created local.settings.json which has the connection string.
 - need to use ngrok to debug locally. (ngrok.io.)
 - https://duckduckgo.com/?q=ngrok+azure+functions&t=brave&ia=web 
