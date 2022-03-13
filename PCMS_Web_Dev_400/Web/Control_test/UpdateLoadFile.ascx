<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UpdateLoadFile.ascx.cs"
    Inherits="UpdateLoadFile" %>
<%@ Import Namespace="PCCore" %>
<!-- search begin -->
<% 
    string UpALLappUrl = Config.AppBaseUrl;
    string UpALLthemeUrl = Config.GetThemeBaseUrl(Page.Theme); 
%>
<div id="divFiles" style="overflow: hidden;  position: absolute; left: auto;
    top: auto; width: auto; height: "50px"; >
    <span runat="server" id="UpdateLoadTable" ><a href="#" runat="server" id="HrefA">
        <!--<PC:Label runat="server" ID="lblUpLabel" Text="<%$ Resources:Labels,UploadFile %>"  LabelStyle="xLabel" />--></a></span>
    <table id="FileTable" runat="server" width="300px" border="0" cellspacing="0" cellpadding="3" >
       <tr>
        <td colspan="4" height="10px;" ></td>
       </tr>   
    </table>
    <table id="<%= this.MyInpuTable %>" style="display: none" border="0" cellpadding="3" cellspacing="0">

        <tr>
            <td  id="<%= this.MyHtmpInputFiles %>"  >
            </td>
           <td >
                 <a id="CloseO"  visible="false" style="vertical-align: middle;" onclick="if(!<%= this.ID%>isULFCheckFile()) return false; <%= this.ID%>InputClose('o');">
                    <asp:Image ID="Image1" visible="false" CssClass="ImgButton" ImageUrl="~/App_Themes/Default/images/UploadFileControl/Upapply.gif"
                        runat="server" />
                </a>&nbsp;&nbsp; 
                <a id="CloseC"  visible="false" style="vertical-align: middle;" onclick="<%= this.ID%>InputClose('c');">
                    <asp:Image ID="Image2" visible="false" CssClass="ImgButton" ImageUrl="~/App_Themes/Default/images/UploadFileControl/Updelete.gif"
                        runat="server" />
                </a>
           </td>
        </tr>
        <tr>
            <td  colspan="2" style="display:none;" >
                <asp:FileUpload ID="Upddd" runat="server"  Enabled=false />
                <PC:TextBox ID="txtNeedDeleteRecord" runat="server" Width="150px" />
                <PC:TextBox ID="txtNeedInsertIndexs" runat="server" Width="150px" />                    
                <PC:TextBox ID="txtHasCount" runat="server" class="Invisible" />
            </td>
        </tr>
    </table>
    
</div>

<script language="javascript" type="text/javascript"  >
    var UpAlldialogFeaturesSearch = "status:no;help:no;scroll:no;dialogHeight:150px;dialogWidth:600px;"; 
    var UpAllSearchformUrl = "<%=UpALLappUrl %>/Control/UploadFile.aspx"; 
    var UpAlldialogFeaturesDownoLoadFile = "status:=no,Height=230,Width=350,top=270,left=320"; 
    var UpAllDownoLoadFileUrl = "<%=UpALLappUrl %>/Control/DownLoadFile.aspx"; 
    var <%= this.ID%>Upo,<%= this.ID%>UpoFileTable;
    var <%= this.ID%>UpRecID;
    var <%= this.ID%>_RequestStr;

    //Display --------begin
     var <%= this.ID%>_FileIndex=0;
     var <%= this.ID%>_HtmlTableRowIndex =0;
     var <%= this.ID%>_AttemntChecked="false";
     var <%= this.ID%>_GogNeetIndex=0; //
    //    function ShowAppendFile()
    //    {
    //         ShowUpLoadFiles('docUpdateLoad','psdocument','txtID');
    //    }
    
    function ShowUpLoadFiles<%= this.ControlsIndex%>(UpobjectName,FuncName,recordID)
    {
        var _strTable;
        var _tmpObj;
         <%= this.ID%>_RequestStr="";
         <%= this.ID%>Upo      = eval(UpobjectName);
         UpFuncName = FuncName;         
         <%= this.ID%>UpRecID  = eval(recordID).value;
         <%= this.ID%>UpoFileTable=eval(UpobjectName+"FileTable");
         
    
         if (<%= this.ID%>UpRecID=="") return ;
           _strTable=UpdateLoadFile.SetPostTable(<%= this.ID%>Upo.UserDefineFuncName,<%= this.ID%>UpRecID,<%= this.ID%>UpoFileTable.outerHTML,"",<%= this.ID%>Upo.UserDefineisDisabled,"<%= this.ID%>","<%= this.ControlsIndex%>").value;
           _tmpObj= eval(<%= this.ID%>UpoFileTable.id);
           _tmpObj.outerHTML=_strTable;
           _tmpObj=null;
           retr=null;
           
       <%= this.ID%>_FileIndex = UpdateLoadFile.GetRecordCount(<%= this.ID%>Upo.UserDefineFuncName,<%= this.ID%>UpRecID ,"<%= this.ControlsIndex%>").value;
       <%= this.ID%>_HtmlTableRowIndex = <%= this.ID%>_FileIndex;
       <%= this.ID%>txtHasCount.value = <%= this.ID%>_FileIndex;
      
    }
    //Display --------end
    
    //dynamic add inpt files----------begin
    function <%= this.ID%>AddNewInputFile(UpobjectName,FuncName,recordID,isMulti)
    {
        var str;
        if (<%= this.ID%>_AttemntChecked=="false")
        {
         <%= this.ID%>Upo      = eval(UpobjectName);
        // <%= this.ID%>Upo.style.display="none";
         ++<%= this.ID%>_FileIndex;
         
         str = '<span id=spanFile'+<%= this.ID%>_FileIndex+' class=xLabel ><%=Resources.Labels.FileName %><span style="width:40px;"></span>:<span style="width:10px;"></span></span><INPUT type="file" class="TextBoxCode1" style="width: 400px;"  NAME="File'+<%= this.ID%>_FileIndex+'" />';
         <%= this.MyInpuTable%>.style.display="";
         <%= this.MyHtmpInputFiles%>.insertAdjacentHTML("beforeEnd",str);
         <%= this.ID%>_AttemntChecked="true";
         
         <%= this.ID%>txtNeedInsertIndexs.value += <%= this.ID%>_GogNeetIndex+",";
         <%= this.ID%>_GogNeetIndex++;
        }
         str=null; 
    }
    
    function <%= this.ID%>AddNewInputFileMultiple(UpobjectName,FuncName,recordID,isMulti)
    {
        var str;
        if (<%= this.ID%>_AttemntChecked=="false")
        {
         <%= this.ID%>Upo      = eval(UpobjectName);
        // <%= this.ID%>Upo.style.display="none";
         ++<%= this.ID%>_FileIndex;
         
         if(isMulti == 1)
            str = '<span id=spanFile'+<%= this.ID%>_FileIndex+' class=xLabel style="width:40px;"><%=Resources.Labels.FileName %><span style="width:40px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>:<span style="width:10px;">&nbsp;&nbsp;</span></span><INPUT type="file" class="TextBoxCode1" style="width: 400px;"  NAME="File'+<%= this.ID%>_FileIndex+'" multiple="multiple"/>';
         else
            str = '<span id=spanFile'+<%= this.ID%>_FileIndex+' class=xLabel style="width:40px;"><%=Resources.Labels.FileName %><span style="width:40px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>:<span style="width:10px;">&nbsp;&nbsp;</span></span><INPUT type="file" class="TextBoxCode1" style="width: 400px;"  NAME="File'+<%= this.ID%>_FileIndex+'"/>';
         <%= this.MyInpuTable%>.style.display="";
         <%= this.MyHtmpInputFiles%>.insertAdjacentHTML("beforeEnd",str);
         <%= this.ID%>_AttemntChecked="true";
         
         <%= this.ID%>txtNeedInsertIndexs.value += <%= this.ID%>_GogNeetIndex+",";
         <%= this.ID%>_GogNeetIndex++;
        }
         str=null; 
    }
    
    function <%= this.ID%>isULFCheckFile()
    {
       var sfileName =  document.getElementById('File'+<%= this.ID%>_FileIndex).value;
       //Save Filename format checking.
       if (!UpdateLoadFile.ULFCheckFile(sfileName).value)
       {
            alert("Bad File path or File Name .Please check .");
            return false;
       }
       return true;
    }
    
    function <%= this.ID%>InputClose(evalue)
    {
        var _strTable;
        var _tmpObj;
        var _fileName="";
        var _fileType="";
        var _iserverFilePath="";
        var oRow,oCellCellIndex,oCellFileTypeICO,oCellFileName,oCellDeleteICO,oCellDeleteLink; 
        
        switch (evalue)
        {
            case "o": 
               document.getElementById('spanFile'+<%= this.ID%>_FileIndex).style.display="none";
               document.getElementById('File'+<%= this.ID%>_FileIndex).style.display="none"; 
               _fileName=document.getElementById('File'+<%= this.ID%>_FileIndex).value;
               
               if (_fileName !="")
               {
                                  
                 ++<%= this.ID%>_HtmlTableRowIndex; 
                 _tmpObj= eval(<%= this.ID%>UpoFileTable.id);     
                 oRow = _tmpObj.insertRow();
                 oRow.id = <%= this.ID%>_FileIndex;  
                 oCellCellIndex = oRow.insertCell();
                 oCellCellIndex.runtimeStyle.display="none";
                 oCellCellIndex.innerHTML=<%= this.ID%>_FileIndex;
                 
                 oCellFileTypeICO = oRow.insertCell();
                 if (_fileName.lastIndexOf(".")>0)
                 {
                    _fileType= _fileName.substring(_fileName.lastIndexOf(".")+1);
                 }
                 else
                 {
                    _fileType="UNKNOW";
                 }
                     _iserverFilePath=UpdateLoadFile.AccessExitFileFullPath(_fileType).value;
               
                     oCellFileTypeICO.innerHTML="<IMG  SRC='"+ _iserverFilePath+"'  border=0>"; 
                     oCellFileTypeICO.runtimeStyle.width="16px";              
     
                  
                  //oCellFileTypeICO.innerHTML="<IMG  SRC='../App_Themes/Default/images/UploadFileControl/IC"+_fileType+".GIF' style=filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='../App_Themes/Default/images/UploadFileControl/ICUNKNOW.GIF',sizingMethod='image')  >";               
                  //filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='/workshop/graphics/earglobe.gif')
                  //oCellFileTypeICO.runtimeStyle.width="16px";
            
                
                 oCellFileName = oRow.insertCell();
                 oCellFileName.innerHTML =  _fileName.substring(_fileName.lastIndexOf("\\")+1);
                 
//                 oCellDeleteICO =oRow.insertCell();
//                 oCellDeleteICO.innerHTML =" <IMG src='../App_Themes/Default/images/UploadFileControl/smalldel.gif' align='absmiddle' border=0 >";
//                 oCellDeleteICO.runtimeStyle.width="16px";
                 
                 oCellDeleteLink=oRow.insertCell();
                 oCellDeleteLink.innerHTML ="<a href=\"#\" class=\"UpCell\" onclick=javascript:<%= this.ID%>RemoveClientRow(\""+ <%= this.ID%>_HtmlTableRowIndex + "\",\""+ <%= this.ID%>_FileIndex + "\"); return false;>" 
                                           + "<IMG src='../App_Themes/Default/images/UploadFileControl/smalldel.gif' align='absmiddle' border=0 >Delete" + "</a>";
                 //oCellDeleteLink.runtimeStyle.width="16px";
                }
              break;
            case "c":
               document.getElementById('spanFile'+<%= this.ID%>_FileIndex).outerHTML=""
               document.getElementById('File'+<%= this.ID%>_FileIndex).outerHTML=""

              break;              
        }
       // <%= this.ID%>Upo.style.display="";   
        <%= this.MyInpuTable%>.style.display="none";
        
         //_strTable=UpdateLoadFile.SetPostTable(<%= this.ID%>Upo.UserDefineFuncName,<%= this.ID%>UpRecID,<%= this.ID%>UpoFileTable.outerHTML,"").value;//display 
         //_tmpObj= eval(<%= this.ID%>UpoFileTable.id);     
         //_tmpObj.outerHTML=_strTable;
        
         <%= this.ID%>_AttemntChecked="false";
         _strTable=null;
         _tmpObj=null;
         _fileName=null;
         _fileType=null;
         
         oRow=null;
         oCellCellIndex=null;
         oCellFileTypeICO=null;
         oCellFileName=null;
         oCellDeleteICO =null;
         oCellDeleteLink=null;
    }

    
    function ShowUpAllForm(UpobjectName,FuncName,recordID)
    {
        var _strTable;
        var _tmpObj;
         <%= this.ID%>_RequestStr="";
         <%= this.ID%>Upo      = eval(UpobjectName);
         UpFuncName = FuncName;         
         <%= this.ID%>UpRecID  = eval(recordID).value;
         <%= this.ID%>UpoFileTable=eval(UpobjectName+"FileTable");

         <%= this.ID%>_RequestStr = UpAllSearchformUrl+"?";
         <%= this.ID%>_RequestStr += "UpObjectName="+<%= this.ID%>Upo+"&UpFuncName="+FuncName +"&UpRecordID="+ <%= this.ID%>UpRecID;
         
         var retr = window.showModalDialog(<%= this.ID%>_RequestStr,"", UpAlldialogFeaturesSearch); 
          
         if (retr !=null)
         {
           _strTable=UpdateLoadFile.SetPostTable(<%= this.ID%>Upo.UserDefineFuncName,<%= this.ID%>UpRecID,<%= this.ID%>UpoFileTable.outerHTML,"","<%= this.ID%>","<%= this.ControlsIndex%>").value;
           _tmpObj= eval(<%= this.ID%>UpoFileTable.id);
           _tmpObj.outerHTML=_strTable;
           _strTable=null;
           _tmpObj=null;
           retr=null;
         }
         
    }
    //dynamic add inpt files----------end
    
    //for client generat script use
    function <%= this.ID%>RemoveClientRow(HtmpRowIndex,FunctionRecordID)
    { 
        var _tmpObj;
        var _TRRowIndex;
        if (event.srcElement.parentElement.parentElement.tagName=="TR")
        {
            _TRRowIndex=event.srcElement.parentElement.parentElement.rowIndex;
            _tmpObj= eval(<%= this.ID%>UpoFileTable.id);
            _tmpObj.deleteRow(_TRRowIndex);
            //_tmpObj.rows[_TRRowIndex].runtimeStyle.display="none";
            
           document.getElementById('spanFile'+FunctionRecordID).outerHTML=""
           document.getElementById('File'+FunctionRecordID).outerHTML=""
           --<%= this.ID%>_HtmlTableRowIndex;
         }
       _tmpObj=null;
       _TRRowIndex =null;
    }
    
    //for server generate script use
    function <%= this.ID%>RemoveLocal(ethis,obj,recordId,HtmpRowid)
    {
         var _tmpObj;
         var _TRRowIndex;
        if (event.srcElement.parentElement.parentElement.tagName=="TR")
        {
          _TRRowIndex=event.srcElement.parentElement.parentElement.rowIndex;
          _tmpObj= eval(<%= this.ID%>UpoFileTable.id);
          
          if (ethis.DorUType != null && ethis.DorUType =="U")
          {         
            <%= this.ID%>ChangeDorUType(ethis,"D");
            <%= this.ID%>txtNeedDeleteRecord.value = <%= this.ID%>txtNeedDeleteRecord.value.replace( recordId + ",","" );
          }
          else
          {
            <%= this.ID%>ChangeDorUType(ethis,"U");
            <%= this.ID%>txtNeedDeleteRecord.value += recordId + ",";
          }
         /*
          _tmpObj.deleteRow(_TRRowIndex);
         <%= this.ID%>txtNeedDeleteRecord.value +=recordId+",";
         --<%= this.ID%>_HtmlTableRowIndex;
         */ 
       }
       _tmpObj=null;
       _TRRowIndex =null;
    }
    
    function <%= this.ID%>ChangeDorUType(e,pType)
    {
        if (e.childNodes != null && e.childNodes.length >0)
        {
            if (e.childNodes[0] !=null && e.childNodes[0].nodeName =="IMG")
            {
                switch (pType)
                {
                    case "D":
                           e.childNodes[0].src = "../App_Themes/Default/images/UploadFileControl/smalldel.gif";
                           e.DorUType ="D";
                           e.parentNode.parentNode.childNodes[2].style.textDecoration = "none"; //Filename
                        break;
                    case "U":
                           e.childNodes[0].src = "../App_Themes/Default/images/UploadFileControl/Upundo.gif";
                           e.DorUType ="U";
                           e.parentNode.parentNode.childNodes[2].style.textDecoration = "line-through"; //Filename
                        break;
                }
            }
            if (e.childNodes[1] !=null && e.childNodes[1].nodeName =="#text")
            {
                switch (pType)
                {
                    case "D":
                           e.childNodes[1].data = "<%=Resources.Common.Delete %>";
                        break;
                    case "U":
                           e.childNodes[1].data = "<%=Resources.Common.Undo %>";
                        break;
                }
            }
            
        }
    }
    
     
//    function  SaveUpdateLoadFileToDB()
//    {
//        UpdateLoadFile.SaveToDateBase();
//    }  
    //----------downLoad  begin
    function <%= this.ID%>DownLoadThis(id)
    {
        if (id !="")
        {
        
           <%= this.ID%>_RequestStr = UpAllDownoLoadFileUrl+"?UpRecordID="+id;
           //window.showModalDialog. not support for download's Respose
           //window.navigate(<%= this.ID%>_RequestStr,"", UpAlldialogFeaturesSearch); //not support save files
           window.open(<%= this.ID%>_RequestStr,"_blank", UpAlldialogFeaturesDownoLoadFile,false); //open a new blank widows,if click open ,open in it 
           //window.open(<%= this.ID%>_RequestStr,"_self", UpAlldialogFeaturesDownoLoadFile,false); //open in self widows,if click open ,open in it 
        }
    }
    //----------download  end
        
</script>

<!-- search end -->
