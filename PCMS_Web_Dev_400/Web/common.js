var PARAM_URL = "url";
var PARAM_MODE = "mode";
var PARAM_ID = "id";
var PARAM_PID = "pid";
var PARAM_WIN = "win";
var PARAM_REPORT = "report";
var PARAM_REPORT_VIEWTYPE = "Preview";

var FORM_MODE_NEW = "New";
var FORM_MODE_EDIT = "Edit";

var COMMAND_CONFIRM = "confirm";
//add by michael, begin
var COMMAND_REVERSE = "reverse";
//end
// Added by Ken, begin (04/05/2015)
var COMMAND_CANCEL = "cancel";
// Added by Ken, end (04/05/2015)
var COMMAND_CLOSE = "close";
var COMMAND_REFRESH = "refresh";
var COMMAND_SAVE = "save";
var COMMAND_SAVEDB = "savedb"
var COMMAND_SUBMIT = "submit";
var COMMAND_CANCEL="cancel";
var COMMAND_DELETE = "delete";
var COMMAND_EXPORT = "export";
var COMMAND_NEW = "new";
var COMMAND_EDIT = "edit";
var COMMAND_IMPORT = "import";
var COMMAND_PRINT  = "print";
var COMMAND_SEARCH  = "search";
var COMMAND_APPLY = "apply";
var COMMAND_REDIRECT = "RedirectURL";

var COMMAND_LOGOUT="logout";
var COMMAND_LOGIN="login";

var FIELD_ID = 0;
var _HROpener = null;
//var DISPLAY_RECORD="Display Record...";
//var ADDING_RECORD="Adding Record...";
//var EDITING_RECORD="Editing Record...";


/////////////////
var TopFrame, ListFrame, TaskFrame, DialogOpener;

var CCPID,CCRecType,CCRecID,CCAllocType;




function Initialize()
{
	window.status = "";
	document.oncontextmenu = ReturnFalse;
	document.ondragstart = ReturnFalse;
	SetTopFrame();
}
function SetTopFrame()
{
	if(self == top)
	{
		TopFrame = self;
	}
	else
	{
		TopFrame = parent;
		var i = 0;

		while(typeof(TopFrame.Toc) != "object" && i < 5)
		{
			TopFrame = TopFrame.parent;
			i++;
		}
	}
}
//////////////////


function initForm() 
{
    SimpleJS.focus1();
    var p = window.dialogArguments;
    if(typeof(p)!="undefined") {
        var opener = p[PARAM_WIN];
        if(typeof(opener)!="undefined")
            _HROpener = opener;
    }
}

function showForm(params, features)
{
    var url = params[PARAM_URL];
    var target = url;
    
    if ( target.indexOf("?") == -1) 
    {
        target += "?";
    }
    else
    {
        target += "&";
    }
    
    var mode = params[PARAM_MODE];
    if(typeof(mode)!="undefined")
        target += "mode=" + mode + "&";

    var id = params[PARAM_ID];
    if(typeof(id)!="undefined")
        target += "id=" + id + "&";

    var pid = params[PARAM_PID];
    if(typeof(pid)!="undefined")
        target += "pid=" + pid + "&";
        
    return SimpleJS.showModalDialog(target, params, features);
}



function showModelessForm(params, features)
{
    var url = params[PARAM_URL];
    var target = "";
    
    var pos= url.lastIndexOf("&")
    
    if( pos >= 0)
        target = url + "&";
    else
        target = url + "?";
    
    var mode = params[PARAM_MODE];
    if(typeof(mode)!="undefined")
        target += "mode=" + mode + "&";

    var id = params[PARAM_ID];
    if(typeof(id)!="undefined")
        target += "id=" + id + "&";

    var pid = params[PARAM_PID];
    if(typeof(pid)!="undefined")
        target += "pid=" + pid + "&";
        
    var report = params[PARAM_REPORT];
    if(typeof(report)!="undefined")
        target += "report=" + report + "&";
        
    var ViewType = params[PARAM_REPORT_VIEWTYPE]; 
     if(typeof(ViewType)!="undefined")
        target += "ViewType=" + ViewType + "&";  
        
        var sOptions;
   
   sOptions = 'status=yes,menubar=no,scrollbars=yes,resizable=yes,toolbar=no';
   sOptions = sOptions + ',width='+(screen.availWidth-10);
   sOptions = sOptions + ',height=' + (screen.availHeight -20) ;
   sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0';   
   
   // return window.showModelessDialog(target, params, sOptions);
    return window.open(target, '_blank', sOptions);

    
}
function OpenWindow( aURL )
{
   var sOptions;

   sOptions = 'status=yes,menubar=no,scrollbars=no,resizable=yes,toolbar=no';
   sOptions = sOptions + ',width='+screen.availWidth;
   sOptions = sOptions + ',height=' + screen.availHeight;
   sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0';
   
 //  window.opener=null;
//   window.close();
   return window.open( aURL, '_blank', sOptions );

   }

function OpenLocationForm(params, features)
{
    var url = params[PARAM_URL];
    var target=url;
    if (url.indexOf("?")==-1){
	    target=url + "?";
	}
	else
	{
        target = url + "&";
    }
    
    var mode = params[PARAM_MODE];
    if(typeof(mode)!="undefined")
        target += "mode=" + mode + "&";

    var id = params[PARAM_ID];
    if(typeof(id)!="undefined")
        target += "id=" + id + "&";
        
    var pid = params[PARAM_PID];
    if(typeof(pid)!="undefined")
        target += "pid=" + pid + "&";     
     if (typeof(features) != "undefined")               
     {
        window.location.href(target+features);
     }
     else
     {
        window.location.href(target);
     }
}

function setSubmit() {
    _HRSubmit = true;
}
function setChange() {
    _HRChange.value="1";
}
function setTextChange(obj)
{
    _HRChange.value="1"; 
}
function clearChange() {
    _HRChange.value="0";
}
function setCommand(cmd) {
    _HRCommand.value = cmd;
}
//add for get client 's txtCommand status 
// like this : document.getElementById("_cmd").value;  
// modi by : jawance 11-13
function getCommand()
{
    return  _HRCommand.value;
}
function setReturnValue(cmd) {
    window.returnValue = cmd;
}
function saveReturnValue(cmd) {
    _HRWinRetVal.value = cmd;
}
function needRefresh(winRetVal) {
    if(winRetVal==COMMAND_REFRESH || _HRWinRetVal.value==COMMAND_REFRESH)
        return true;
    return false;
}
function setSaveClicked() {
    _HRSave.value="1";
}
function isSaveClicked() {
    return (_HRSave.value=="1");
}
function closeForm() 
{
    if(typeof(onCloseForm)=="function") {
        if(!onCloseForm()) return false;
    }
    setCommand(COMMAND_CLOSE);
   
    if(isSaveClicked()) setReturnValue(COMMAND_REFRESH);
    else setReturnValue(COMMAND_CLOSE);
    window.close();
    return false;
}
function closeFormByReturnValue(ReturnValue)
{
    if(typeof(onCloseForm)=="function") {
        if(!onCloseForm()) return false;
    }
    setCommand(COMMAND_CLOSE);
    setReturnValue(ReturnValue);
    
    window.close();
    return false;
}
function saveForm() 
{   
    
    if(typeof(onSaveForm)=="function") {
        if(!onSaveForm()) return false;
    }
    setCommand(COMMAND_SAVE);
    setReturnValue(COMMAND_REFRESH);
    if(_HROpener!=null)
        _HROpener.saveReturnValue(COMMAND_REFRESH);
    setSaveClicked();
    //Test Point
    
    clearChange();
    return true;
}
function dataChanged() {
    return _HRChange.value=="1";
}
function checkChange() {
    if(dataChanged()) {
        window.event.returnValue = MSG_DATA_CHANGED;
    }
}
function JumpLang(pLang) {
	var tmpUrl=window.location.toString();
	//alert(tmpUrl);
	tmpUrl=tmpUrl.replace("#","");
	var url;
	var tmpInt;
	var tmpStr;

	
	if (tmpUrl.indexOf("?")==-1){
	    url=tmpUrl+"?lang="+pLang;
	}
	else
	{
	    tmpInt=tmpUrl.indexOf("lang=");	    
	    if(tmpInt==-1){
	        url=tmpUrl+"&lang="+pLang;
	    }
	    else
	    {
	        tmpStr= tmpUrl.substring(tmpInt,tmpInt+10);
	        url=tmpUrl.replace(tmpStr,"lang="+pLang);
	    }
	}
		
	window.navigate(url);
}

//处理隐藏在的页面控件的获取焦点问题
function tryFocus(obj)
{
    try
    {
        obj.focus();
    }
    catch(ex){}
}

function BannerLogin(){    
//    frm=document.forms[0];
//    setCommand(COMMAND_LOGIN);
//    frm.submit();

    if(typeof(txtLoginName)!="undefined")
    {
        txtLoginName.focus();
    }
    else
    {
        window.navigate("Default.aspx");
    }
    
}

function Logout(){    
    frm=document.forms[0];
    setCommand(COMMAND_LOGOUT);
    frm.submit();
}

function f_open_window_max( aURL, aWinName ,winScrollWidth,winScrollHeight)
{
   var wOpen;
   var sOptions;
   sOptions = 'fullscreen=0,status=yes,menubar=no,scrollbars=no,resizable=yes,location=0,toolbar=no';
   if (winScrollWidth != null )
   {
        sOptions = sOptions + ',width=' + (winScrollWidth -10);
   }else
   {
        sOptions = sOptions + ',width=' + (screen.availWidth-10);
   }
   
   if (winScrollHeight != null )
   {
        sOptions = sOptions + ',height=' + (winScrollHeight -20);
   }else
   {
        sOptions = sOptions + ',height=' + (screen.availHeight-20);
   }
   
   //sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0';
   sOptions = sOptions + 'scrollbars,screenX=0,screenY=0,left=0,top=0';
   
  // window.opener=null;
   wOpen = window.open( aURL, aWinName, sOptions );
   //window.opener='_null';
//   wOpen.location = aURL;
  //window.close();
//   wOpen.focus();
//   wOpen.moveTo( 0, 0 );
//   wOpen.resizeTo( screen.availWidth, screen.availHeight );
   
   return false;
   }
   
function ShowReport(rpt,arg)
    {
       
        f_open_window_max("RptViewer.aspx?rpt=" + rpt + "&args=" + arg,"_blank");
    }
    
function ShowXtraReport(rptName,arg)
    {
        f_open_window_max( rptName + arg,"_blank");
    }
     /*     
function GoStaff() {
   
   var _src="../PersonalQuery.aspx";
   var tmpUrl = window.location.href;
   var _staff=frmMain.txtStaffSearch.value;  
   var _backUrl=SimpleJS.getUrlKeyValue("src");   
   if (tmpUrl.indexOf("PersonalQuery.aspx")==-1)
   {        
        window.location.href(_src+"?src="+tmpUrl+"&staff="+_staff);
   }
   else
   {        
        var url="PersonalQuery.aspx?src="+_backUrl+"&staff="+_staff;
        window.location.href(url);    
   }  
   
    
} */ 

function AdvSearch() {
//    var tmpUrl = window.location.href;
//    var _backUrl=SimpleJS.getUrlKeyValue("src");
//    if (tmpUrl.indexOf("PersonalQuery.aspx")==-1)
//    {
//        window.location.href("../PersonalQuery.aspx?src="+tmpUrl)
//    }
//    else
//    {
//        window.location.href("PersonalQuery.aspx?src="+_backUrl)
//    }




    var tmpUrl = window.location.href;    
    var _backUrl=SimpleJS.getUrlKeyValue("src");
   
    if (tmpUrl.indexOf("Admin_Default.aspx")==-1 && tmpUrl.indexOf("PersonalQuery.aspx")==-1 && tmpUrl.indexOf("Default.aspx")==-1)
    {       
        window.location.href("../AdvancedSearch_Staff.aspx")
    }
    else
    {       
        window.location.href("AdvancedSearch_Staff.aspx")
    }
    
}

  function HrRowCheckAll(RowIndex,eAll,eQry,eNew,eEdit,eDel,eLogin,ePub)
  {
       if (document.getElementById(eAll).checked)
       {
        if (eQry!="")
            document.getElementById(eQry).checked='checked';
        if (eNew!="")
            document.getElementById(eNew).checked='checked';
        if (eEdit!="")
            document.getElementById(eEdit).checked='checked';
        if (eDel!="")
            document.getElementById(eDel).checked='checked';
        if (eLogin!="")
            document.getElementById(eLogin).checked='checked';
        if (ePub!="")
            document.getElementById(ePub).checked='checked'; 
        eval(gvData.onRowChanged(RowIndex));         
       }
       else
       {
        if (eQry!="")
            document.getElementById(eQry).checked='';
        if (eNew!="")
            document.getElementById(eNew).checked='';
        if (eEdit!="")
            document.getElementById(eEdit).checked='';
        if (eDel!="")
            document.getElementById(eDel).checked='';
        if (eLogin!="")
            document.getElementById(eLogin).checked='';
        if (ePub!="")
            document.getElementById(ePub).checked='';  
        eval(gvData.onRowChanged(RowIndex));                        
       }         
  }
  
  
  function HrFunctionCheckOne(RowIndex,eAll,eQry,eNew,eEdit,eDel,eLogin,ePub)
  {
       if (document.getElementById(eQry).checked 
                && document.getElementById(eNew).checked 
                    && document.getElementById(eEdit).checked 
                        && document.getElementById(eDel).checked 
                            && document.getElementById(eLogin).checked
                                && document.getElementById(ePub).checked 
                                    ) 
       {
        document.getElementById(eAll).checked='checked';       
        eval(gvData.onRowChanged(RowIndex));
       }
       else
       {
        document.getElementById(eAll).checked='';  
        eval(gvData.onRowChanged(RowIndex));                    
       }      
              
  }
  
  function HrGroupRightCheckOne(RowIndex,eAll,eQry,eNew,eEdit,eDel)
  {
       if (document.getElementById(eQry).checked 
                && document.getElementById(eNew).checked 
                    && document.getElementById(eEdit).checked 
                        && document.getElementById(eDel).checked   ) 
       {
        document.getElementById(eAll).checked='checked';       
        eval(gvData.onRowChanged(RowIndex));
       }
       else
       {
        document.getElementById(eAll).checked='';  
        eval(gvData.onRowChanged(RowIndex));                    
       }      
              
  }  
  
  function HrUserRightCheckOne(RowIndex,eAll,eQry,eNew,eEdit,eDel)
  {
       if (document.getElementById(eQry).checked 
                && document.getElementById(eNew).checked 
                    && document.getElementById(eEdit).checked 
                        && document.getElementById(eDel).checked   ) 
       {
        document.getElementById(eAll).checked='checked';       
        eval(gvData.onRowChanged(RowIndex));
       }
       else
       {
        document.getElementById(eAll).checked='';  
        eval(gvData.onRowChanged(RowIndex));                    
       }      
              
  } 
  
  function HrCheckCode(RowIndex,eCode,eName)
  {
        if (document.getElementById(eCode).checked)
        {
          document.getElementById(eCode).checked='checked';            
          document.getElementById(eName).checked=''; 
        }
        else
        {
          document.getElementById(eCode).checked='';            
          document.getElementById(eName).checked='checked'; 
        }

        eval(gvData.onRowChanged(RowIndex));                             
  }   
  function HrCheckName(RowIndex,eCode,eName)
  {
        if (document.getElementById(eName).checked)
        {
          document.getElementById(eCode).checked='';            
          document.getElementById(eName).checked='checked'; 
        }
        else
        {
          document.getElementById(eCode).checked='checked';            
          document.getElementById(eName).checked=''; 
        }

                        
        eval(gvData.onRowChanged(RowIndex));
  }     
  
  function chan()
	{
		for(i=0;i<mytable.children[0].children.length;i++)
			mytable.children[0].children[i].className="chg00";
	}

function chgcolor(obj)
{
	for (i=0;i<mytable.children[0].children.length;i++)
                mytable.children[0].children[i].className="chg00";
		obj.className="chgsel";
}	

function GetDateValueByFormat(GetYMD,objectvalue)
{
    //** use Sample : 2006-01-01 / 2006/06/01
    //** GetDateValueByFormat("Y",yourobject) => 2006
    
    var newString,sValue,RegStr,SplStr;
    RegStr= /([0-9])/g; //全部数字
    
    SplStr =objectvalue.replace(RegStr,"").substring(0,1);//获得分割符号
   
    newString=objectvalue.split(SplStr);        
    switch (GetYMD)
    {
        case "Y":
            sValue=newString[0]*1;
            return sValue;
        break;
        case "M":
            sValue=newString[1]*1;
            return sValue;
        break;
        case "D":
            sValue=newString[2]*1;
            return sValue;
        break;
                       
        default:
            return 0;
        break;
        
    }
    
}


function GetDateValueByDateObj(DateObject)
{
    //return xxxx-xx-xx    
        var sReturn ="";
        if (DateObject!=null && typeof(DateObject) == "object" )
        {
            sReturn = DateObject.getYear() + "-" + (DateObject.getMonth() + 1) + "-" + DateObject.getDate();
        }
    return sReturn;
}

function SetDateFormat(format,DateObjectValue)
{
    //use Sample : 2006/06/01
    //SetDateFormat("-",yourobject) => 2006-06-01
    var _DateObject; 
    var _Date,Y,M,D;
    
    _DateObject = new Date(GetDateValueByFormat("Y",DateObjectValue),
                GetDateValueByFormat("M",DateObjectValue) - 1,GetDateValueByFormat("D",DateObjectValue));
    
    Y= _DateObject.getFullYear();
    M= _DateObject.getMonth() + 1;
    D= _DateObject.getDate();

    if (M.toString().length < 2)
    {
        M="0"+M;
    }
    if (D.toString().length < 2)
    {
        D="0"+D;
    }
     switch (format)
    {
        case "-":
            _Date=Y +"-"+ M +"-" +D;
            return _Date;
        break;
        case "/":
            _Date=Y +"/"+ M +"/" +D;
            return _Date;
        break;      
    }
}

function GetDateNowYMD()
{   
    var _Date,Y,M,D;
    _Date = new Date();
   
    Y=_Date.getFullYear();
    M=_Date.getMonth() + 1;
    D=_Date.getDate();
   
    if (M.toString().length < 2)
    {
        M="0"+M;
    }
    if (D.toString().length < 2)
    {
        D="0"+D;
    } 
   _Date= Y +"-"+ M +"-"+ D;
   
   return _Date;
}

function GridCheckBox(index)
{
       rows=gvData.GridView.rows;
       MasterCheckBox=rows(0).cells(index).children[0];
       //alert(MasterCheckBox.checked);
       for(var i=1; i<rows.length; i++) {
            var chk = rows(i).cells(index).children[0];
          			   if (MasterCheckBox.checked)
					   {
					    chk.checked=MasterCheckBox.checked;	  
					    eval(chk.onclick());  
					   }else{
					    chk.checked=MasterCheckBox.checked;	 
					    eval(chk.onclick());
					   }
					   
       }
}

function SimpleGridCheckBox(index)
{
       rows=gvData.GridView.rows;
       MasterCheckBox=rows(0).cells(index).children[0];
       for(var i=1; i<rows.length; i++) {
            var chk = rows(i).cells(index).children[0];
            
            if(chk.disabled==false)
            {
                if(MasterCheckBox.value=="1" && chk.value!="2")
                {
                    chk.value="2";
                    eval(chk.children[0].onclick());
                } 
                if(MasterCheckBox.value=="2" && chk.value!="0")
                {
                    chk.value="0";
                    eval(chk.children[0].onclick());
                } 
                if(MasterCheckBox.value=="0" && chk.value!="1")
                {
                    chk.value="1";
                    eval(chk.children[0].onclick());
                }  			   
		        //chk.value=MasterCheckBox.value;	 
		        //gvData.onRowClicked(i);
		        
		    }
		    
       }
}
function SimpleImageCheckBoxHeader(index)
{
       //alert("a");
       rows=gvData.GridView.rows;
       //alert(rows(0).cells.Count);
       
       MasterCheckBox=rows(0).cells(index).children[0];
       //alert(rows.length);
       for(var i=1; i<rows.length; i++) {
            try
            {
                //alert(i);
                
                var chk = rows(i).cells(index).children[0];
               
                
                if(chk.enabled)
                {
            
                    chk.value=MasterCheckBox.value;
                    eval(chk.click());
		        }
		    } catch (e)
		    {
		       //alert(e);
		    }
		    
       }
}
function GridCheckBox2(gvData,index)
{          
       var gvDataObj;
       gvDataObj= frmMain.document.getElementById(gvData.id);       
       rows=gvDataObj.rows;
       MasterCheckBox=rows(0).cells(index).children[0];
       //alert(MasterCheckBox.checked);
       for(var i=1; i<rows.length; i++) {
            var chk = rows(i).cells(index).children[0];            
            if (typeof(chk)=='object') {
          			   if (MasterCheckBox.checked)
					   {
					    chk.checked=MasterCheckBox.checked;	  
					    eval(chk.onclick());  
					   }else{
					    chk.checked=MasterCheckBox.checked;	 
					    eval(chk.onclick());
					   }
			}		   
       }
}

//add for Personal Termination  2007-07-02
function GridDynamicCellCheckBox(pRowIndex,pCellBeginIndex,pCellIndex)
{
     
       var rows= gvData.GridView.rows(pRowIndex);
       var MasterCheckBox=rows.cells(pCellIndex).children[0];
       for(var i= pCellBeginIndex; i <rows.cells.length; i++) 
       {
            var chk = rows.cells(i).children[0];
          			   if ( i == pCellIndex)
					   {
					     chk.checked=MasterCheckBox.checked;	   
					   }
					   else{
					    chk.checked=false; 
					   }
       }
}
function GridDynamicCellCheckBoxV2(pRowIndex,pCellBeginIndex,pCellIndex)
{
       var rows= gvData.GridView.rows(pRowIndex);
       var MasterCheckBox=rows.cells(pCellIndex).children[0].children[0];
       for(var i= pCellBeginIndex; i <rows.cells.length; i++) 
       {
            var chk = rows.cells(i).children[0].children[0];
          			   if ( i == pCellIndex)
					   {
					     chk.checked=MasterCheckBox.checked;	   
					   }
					   else{
					    chk.checked=false; 
					   }
       }
}

function GridDynamicCellCheckBoxV3(pRowIndex,pCellBeginIndex,pCellEndIndex,pCellIndex)
{
       var rows= gvData.GridView.rows(pRowIndex);
       var MasterCheckBox=rows.cells(pCellIndex).children[0].children[0];
       for(var i= pCellBeginIndex; i <pCellEndIndex; i++) 
       {
            var chk = rows.cells(i).children[0].children[0];
          			   if ( i == pCellIndex)
					   {
					     chk.checked=MasterCheckBox.checked;	   
					   }
					   else{
					    chk.checked=false; 
					   }
       }
}

function GridDynamicCellCheckBoxV4(pRowIndex,pCellBeginIndex,pCellEndIndex,pCellIndex,TextBoxValue,havescore,RatinggvData,Score)
{

       var result=0;
       var rows= RatinggvData.GridView.rows(pRowIndex);
       var MasterCheckBox=rows.cells(pCellIndex).children[0].children[0];
       
       var ScoreRows=RatinggvData.GridView.rows.length-1;
       var txtBox=RatinggvData.GridView.rows(ScoreRows).cells(pCellEndIndex-1).children[0];
       var TotalValue;

       for(var i= pCellBeginIndex; i <pCellEndIndex; i++) 
       {
            var chk = rows.cells(i).children[0].children[0];
            
            if(i==pCellEndIndex-1)
            {
                chk=rows.cells(i).children[0];
            }
            if(chk.type=="checkbox")
            {
          			   if ( i == pCellIndex)
					   {
					     chk.checked=MasterCheckBox.checked;	 
  
					   }
					   else{
					    chk.checked=false; 
					   }            
            }else if(chk.type=="text"  && havescore=="1")
            {
                chk.Text=TextBoxValue;
                chk.value=TextBoxValue;
            }
            if(chk.type=="checkbox" && chk.checked)
            {
                    txtBox.value="";

                    var nvalue=0;
                    for(var k=1;k<RatinggvData.GridView.rows.length-1;k++)
                    {
                        
                        if(pRowIndex == k)
                        {
                            RatinggvData.GridView.rows[k].cells[pCellEndIndex-1].all(0).value=TextBoxValue;
                            //nvalue =parseInt(nvalue)+parseInt(RatinggvData.GridView.rows[k].cells[pCellEndIndex-1].all(0).value);
                            
                        } 
                        if(RatinggvData.GridView.rows[k].cells[pCellEndIndex-1].all(0).value=="")
                        {
                            RatinggvData.GridView.rows[k].cells[pCellEndIndex-1].all(0).value=0;
                        }
                        nvalue =parseInt(nvalue)+parseInt(RatinggvData.GridView.rows[k].cells[pCellEndIndex-1].all(0).value);                           
                                                
                    }

                    txtBox.value = nvalue;
                
            }

       }
}
function GridDynamicCellTextBoxOnchange(pRowIndex,pCellBeginIndex,pCellEndIndex,pCellIndex,TextBoxValue,havescore,RatinggvData,Score,txtSaveBox,FormerlyValue)
{


       var rows= RatinggvData.GridView.rows(pRowIndex);
       var chk;
       var lbl=rows.cells(4).children[0];
       var typeid=RatinggvData.GridView.attributes.RatingTypeID.value;
       var lblvalue=lbl.innerText;
       var Str1;
       var Str2;
       
         txtSaveBox.value+=typeid+","+lblvalue; 
         
       if(Score==-1)
       {
            if(pCellEndIndex==8)
            {
                txtSaveBox.value+=","+rows.cells[pCellEndIndex].children[0].value+";";
            }
       }
       else
       {
            for(var i= pCellBeginIndex; i <=pCellEndIndex; i++) 
            {
        
                    if(i<=pCellEndIndex-2)
                    {
                     chk = rows.cells(i).children[0].children[0];
                    }
                    else
                    {
                     chk=rows.cells(i).children[0];
                    }

                    if(chk.type=="checkbox" && chk.checked==true)
                    {
                      txtSaveBox.value+=","+chk.TTT;
                    }
                    if(havescore=="1" && i==pCellEndIndex-1)
                    {

                        txtSaveBox.value+=","+chk.value;
                    }else if(i==pCellEndIndex)
                    {
                        txtSaveBox.value+=","+chk.value+";";
                    }
       }
       }  
       
       //处理同一条记录第2次变动,取最后的改动
       if(txtSaveBox.value.split(";").length>0)
       {
            for(var i=0;i<txtSaveBox.value.split(";").length-2;i++)
            {
                if(txtSaveBox.value.split(";")[i].split(",").length-2>0)
                {
                    Str1=txtSaveBox.value.split(";")[i].split(",")[0];
                    Str2=txtSaveBox.value.split(";")[i].split(",")[1];
                    if((typeid+lblvalue)==(Str1+Str2))
                    {
                        txtSaveBox.value=txtSaveBox.value.substr(txtSaveBox.value.indexOf(txtSaveBox.value.split(";")[i]))
                        //alert(txtSaveBox.value)
                    }
                }
            }

       }
              

}

function GridDynamicCellTextV4(pRowIndex,pCellBeginIndex,pCellEndIndex,pCellIndex,TextBoxValue,havescore,RatinggvData)
{
       var rows= RatinggvData.GridView.rows(pRowIndex);
       var MasterCheckBox=rows.cells(pCellIndex).children[0].children[0];
       for(var i= pCellBeginIndex; i <pCellEndIndex; i++) 
       {
            var chk = rows.cells(i).children[0].children[0];
            if(i==pCellEndIndex-1)
            {
                chk=rows.cells(i).children[0];
            }
            if(chk.type=="checkbox")
            {
                if(chk.checked=="true")
                {
                    
                }        
            }else if(chk.type=="text"  && havescore=="1")
            {
                //if(chk.checked=="true")
                chk.Text=TextBoxValue;
                chk.value=TextBoxValue;
            }

       }
}


function setGradeLevel(Gradeobj,Gradelevelobj,JsArray,OpDefault,OpDefaultValue,ColumnCount)
{
    
    var GselectValue =null;
    var OptionValue  =null;
    var GLselectedValue=null;
    GselectValue=Gradeobj.value;
    GLselectValue=Gradelevelobj.value;
    //clear ddl
    Gradelevelobj.length=0;
    //set Default Option
    Gradelevelobj.options.add(new Option(OpDefault,OpDefaultValue));
    //from 1 begin ,0 is record count
    var GLindex = 0;
    if (JsArray == null || JsArray.length == 0) return;     
    for (var i=1; i<JsArray.length;i=i+ColumnCount)
    {
        if (GselectValue==JsArray[i+1])
        {
            GLindex +=1;
           Gradelevelobj.options.add(new Option(JsArray[i+2],JsArray[i]));
           Gradelevelobj.options[GLindex].amount=JsArray[i+3];
           if (JsArray[i]==GLselectValue)
           {
               Gradelevelobj.selectedIndex=GLindex; 
           }
        }          
    }
}
function FireSetGredeLevel(obj,objLevel)
{
   eval(obj.onfocus());
}


function setPositionLevel(Positionobj,Positionlevelobj,JsArray,OpDefault,OpDefaultValue,ColumnCount)
{
    var PselectValue =null;
    var OptionValue  =null;
    var PLselectedValue=null;
    PselectValue=Positionobj.value;
    PLselectValue=Positionlevelobj.value;
    //clear ddl
    Positionlevelobj.length=0;
    //set Default Option
    Positionlevelobj.options.add(new Option(OpDefault,OpDefaultValue));
    //from 1 begin ,0 is record count
    var PLindex = 0;
    if (JsArray == null || JsArray.length == 0) return;
    for (var i=1; i<JsArray.length;i=i+ColumnCount)
    {
        if (PselectValue==JsArray[i+1])
        {
            PLindex +=1;
           Positionlevelobj.options.add(new Option(JsArray[i+2],JsArray[i])); 
           if (JsArray[i]==PLselectValue)
           {
               Positionlevelobj.selectedIndex=PLindex; 
           }
        }          
    }
}
function FireSetPositionLevel(obj,objLevel)
{

   try
   {
    eval(obj.onchange());
   }
   catch(ex)
   {
   }

}
//************
//========== 以下是通用的2级联动代码==============
//************
function SetChildValue(Parentobj,Childobj,JsArray,OpDefault,OpDefaultValue,ColumnCount)
{
    var ParentselectValue =null;
    var OptionValue  =null;
    var ChildselectedValue=null;
    ParentselectValue=Parentobj.value;
    ChildselectedValue=Childobj.value;
    //clear ddl
    Childobj.length=0;
    //set Default Option
    Childobj.options.add(new Option(OpDefault,OpDefaultValue));
    //from 1 begin ,0 is record count
    var PLindex=0;       
    for (var i=1; i<JsArray.length;i=i+ColumnCount)
    {
        if (ParentselectValue==JsArray[i+1])
        {
           PLindex +=1;
           Childobj.options.add(new Option(JsArray[i+2],JsArray[i])); 
           if (JsArray[i]==ChildselectedValue)
           {
               Childobj.selectedIndex=PLindex; 
           }
        }          
    }
}
function FireSetChildValue(obj,objLevel)
{

   eval(obj.onchange());

}

//end 2级联动代码

function HrAutoSetTimeHHMM(obj)
{
    var tmp;
    var hours;
    var minutes;
    var hour,min,timeArray;
    var nextday;
    nextday=0;
    tmp=obj.value;
    //alert(tmp);
    if(tmp==null || tmp=="") return "";
    if(tmp.indexOf(':')<2 && tmp.indexOf(':')>-1)
    {
        hours="00" + tmp.substring(0,tmp.indexOf(':')) + ":";
        //alert(hours);
        hours=hours.substring(hours.indexOf(':')-2,hours.indexOf(':'));
        //alert(hours);
        minutes=tmp.substring(tmp.indexOf(':')+1,tmp.indexOf(':')+3);
        //alert(minutes);
        tmp=hours+":"+minutes;
    }
    tmp=tmp.replace(":","");
    hour=tmp.substring(0,2);
    min=tmp.substring(2,4);
//    alert(tmp);
//    alert(hour);
//    alert(min);
    
//    timeArray=obj.value.split(":");
//    if (timeArray.length<0)
//        return ;
//    hour=timeArray[0];
//    min=timeArray[1];  

    hour=hour % 36;
    //if (hour==0) hour=0;
    //alert(hour);
    nextday=parseInt(hour / 24);
    

    if (typeof(hour)=="undefined")
        hour="12" ;    
    if (typeof(min)=="undefined")
        min="00" ;
    var newDate = new Date();
    newDate.setHours(hour,min,0,0);
    
    if (isNaN(newDate)){
        obj.value="00:00";
        return;
    }
    if ((newDate.getHours()+nextday*24).toString().length==1)
    {
        hour="0"+(newDate.getHours()+nextday*24).toString();
    }else{
        hour=(newDate.getHours()+nextday*24).toString();
    }
    if (newDate.getMinutes().toString().length==1)
    {
        min="0"+newDate.getMinutes().toString();
    }else{
        min=newDate.getMinutes().toString();
    }
    
    obj.value=hour+":"+min;
    if(obj.newChange == "True")
    {
        setnewChange();
    }
   
}



    
   function SetCurrencyFormat(e)
    {   
    //no ues 
        var evalue= e.value;   
        var nStr;
        
        if (!SimpleJS.isDouble(evalue))
        {
            e.value="0";
            return false;
        }
        var re = new RegExp("^\\s*([-\\+])?\\$?(\\d{1,3},)((\\d{3},)*)(\\d{3})(\\.\\d+)?\\s*$");
        var m = evalue.match(re);
       //alert(m.length);     
    }
    
    function InputOnlyCurrency()
    {   
        return;        
        if (!((window.event.keyCode>=48 && window.event.keyCode<=57) || (window.event.keyCode>=96 && window.event.keyCode<=105) || window.event.keyCode==13 || window.event.keyCode==8 || window.event.keyCode==46)) 
            {window.event.keyCode=8;
            return false;}
    
    }
    
    function InputOnlyInteger()
    {
         if (!((window.event.keyCode>=48 && window.event.keyCode<=57) || (window.event.keyCode>=96 && window.event.keyCode<=105) || window.event.keyCode==13 || window.event.keyCode==8 || window.event.keyCode==46)) 
            {return false;}
    }
    
    //add by kamte at 2006-07-05
    function ChangeCurrency(newCurrency,CurrencyName)
       {          
            
            if(SimpleJS.isNullOrEmpty(newCurrency)) return "";            
            var re = /([^(\d|.|\-)])/g;
            var ree = /([^0-9])/g;
            var returnValues = repalceValue(newCurrency,re,"");
            var checkValue = repalceValue(newCurrency,ree,"");
            if(SimpleJS.isNullOrEmpty(checkValue)) return ""; 
            
            
            try
            {   returnValues = GetrealValue(returnValues);
                var intValue = parseInt(returnValues,10); 
                var repValue = FormatValue(intValue.toString());      
                //returnValues = returnValues.replace(intValue.toString(),repValue.toString());
                var rel = /.$/g;
                if(returnValues.substring(returnValues.length-1,returnValues.length) == ".")
                {
                    returnValues = returnValues.replace(rel,"");
                }
                
                if(returnValues.indexOf(".") != -1)
                {
                    var reText = returnValues.substring(returnValues.indexOf("."),returnValues.length);
                    if(returnValues.indexOf(".") == 0)
                    {
                        returnValues = "0" + returnValues; 
                    }else
                    {
                        returnValues =  repValue + reText;
                    }
                }else
                {
                    returnValues =  repValue;
                } 
                            
                
                returnValues = returnValues + CurrencyName;
                
            }catch(e)
            {
                if (returnValues=="") returnValues="0";
                return returnValues;
            }finally
            {
            }
            //alert(returnValues);
            if (returnValues=="") returnValues="0";
            return returnValues;
       }
       
      function GetrealValue(OldValue)
       {
         while(OldValue.indexOf(".") != OldValue.lastIndexOf("."))
         {
            OldValue = OldValue.substring(0,OldValue.lastIndexOf("."));
         }
         
         return OldValue;
       }
       
       function repalceValue(OldValue,regExp,replaceText)
       {       
           var newValue = OldValue.replace(regExp,replaceText);
           try
           {
           if(regExp != replaceText)
           {
               while(newValue.indexOf(regExp) > -1)
               {
                newValue = newValue.replace(regExp,replaceText);
               }
            }
           }catch(e)
           {
           }finally
           {
           }
           
           return newValue; 
       }
       function FormatValue(OldValue)
       {
            var pre="";
           if(OldValue.substring(0,1)=="-")
           {
            pre="-";
           }
           var newValue = OldValue.replace("-","");
           var reValue = "";
           while(newValue.length > 3)
           {
                reValue = "," + newValue.substring(newValue.length-3,newValue.length) + reValue;
                newValue = newValue.substring(0,newValue.length-3);         
           } 
           
           reValue = pre + newValue + reValue;
           return reValue;
       }
       
   function ChangePercent(PercentValue,FormatValue)
   {
     var reg = /[0-9]{0,2}[\\.]{0,1}[0-9]{1,2}/g;
     var NewValue,ReturnValue;   
        ReturnValue="";
     if(SimpleJS.isNullOrEmpty(PercentValue)) 
        return "";
        
        NewValue =PercentValue.match(reg);
        
     if(NewValue==null) 
        return "";   
    
     if (NewValue.length >= 2 )// 超过2个，剔除
        return "";
        
//     if ((NewValue/100) > 1 ) //123 /100 大于100%， 剔除
//        return "";
     //alert(NewValue[0]);
     if (NewValue[0].indexOf(".")==0) //.99 => 0.99
     {
        return "0"+NewValue[0]+FormatValue;
        }
     else{
        return NewValue[0]+FormatValue;
     }   
     //NewValue= NewValue.replace(ree,"");  
    // if(SimpleJS.isNullOrEmpty(NewValue)) return ""; 
            
    //return  ReturnValue;           
            
   }
   
   function ChangeBankAccount(evalue)
   {    
     var reg=/[0-9]{15,15}/g;
     var NewValue =evalue.match(hi );
     
    if(NewValue==null) 
        return "";   
    return NewValue[0];
     
   }
   
   function GetAges(BirthDate,CompDate)
   {
    var BirthYear,BirthMonth,CompYear,CompMonth;
    var Years,Months;
    var boolCthanB; //BirthDate'month < ComparDate'month
    if ((!SimpleJS.isDateYMD(BirthDate)) || (!SimpleJS.isDateYMD(CompDate))) return "0 "+"Year(s)"+"0 "+"Month(s)";
    
    BirthYear=GetDateValueByFormat("Y",BirthDate);
     CompYear=GetDateValueByFormat("Y",CompDate);
      BirthMonth=GetDateValueByFormat("M",BirthDate);
       CompMonth=GetDateValueByFormat("M",CompDate);
    
    if (CompMonth >= BirthMonth) boolCthanB=true;
    
    if (boolCthanB)
    {
      Years  = CompYear - BirthYear ;
      Months = CompMonth -BirthMonth;
    }else {
      Years  = CompYear - BirthYear -1;
      Months = CompMonth +12 - BirthMonth;
    }

    return  Years + " Year(s) " + Months +" Month(s)";
   }
 
 
 HRJS = function() {

 } 
 HRJS.isDateYMD = function(s)
 {
    var valid = false;
    var re = new RegExp("^\\s*(\\d{4})([-])(\\d{1,2})\\2(\\d{1,2})\\s*$");
    //[1,1]=>   $0=1106-02-01   $1=1106   $2=1106   $3=   $4=-   $5=02   $6=01
    //[1,1]=>   $0=1198-01-06   $1=1198   $2=-   $3=01   $4=06
    var m = s.match(re);
    if (m != null) 
    {
        var day, month, year;
        day = m[4];
        month = m[3];
        year = m[1];
        month -= 1;
        var dt = new Date(year, month, day);
        valid = (typeof(dt) == "object" && year == dt.getFullYear() && month == dt.getMonth() && day == dt.getDate()) ? true : false;
    }
    return valid;
}


//add by kamte
function keypress(obj)
{
 var kindex = window.event.keyCode;
 if(kindex < 48 || kindex >57)
 {
  window.event.keyCode = 0;
  return;
 }

 var ovalue = obj.value;
 var re = document.selection.createRange();
 re.text = String.fromCharCode(kindex);
 var nvalue = obj.value;  
 document.selection.clear();
 obj.value = nvalue;
 
 switch(nvalue.length)
 {
  case 1:if(kindex > 51)
   obj.value = "0" + nvalue + ":";   
   break;
  case 2:if(nvalue.indexOf(":") > -1)
				  {
				  	if(nvalue.indexOf(":") == 0)
				  	{
				  		if(kindex > 53)
				  			obj.value = "00:0"+String.fromCharCode(kindex);
				  		else
				  			obj.value = "00"+nvalue;	
				  	}
				  	else
				  		{
				  			if(kindex > 53)
				  				obj.value = "0"+nvalue;
				  			else
				  				obj.value = nvalue;
				  		}
				  }else
				  	{
				  	if(parseInt(nvalue,10)>36)
					   	obj.value = ovalue;
					   else	 
					   	obj.value = nvalue + ":";
	  			}	  	
   break;
  case 3:check3value(obj,ovalue,nvalue);
   break;
  case 4:check4value(obj,ovalue,nvalue);
   break;
  case 5:check5value(obj,ovalue,nvalue);
   break;
  default:break;
 }
 //obj.focus();
 window.event.keyCode = 0;
}
function check5value(obj,ovalue,nvalue)
{
 var i = nvalue.indexOf(":");
 nvalue = nvalue.replace(":","");
 switch(i)
 {
  case 2:if(parseInt(nvalue,10) >3600)
   obj.value = ovalue;   
   break;
  default:obj.value = ovalue;
  break;
 }
}
function check4value(obj,ovalue,nvalue)
{
 var i = nvalue.indexOf(":");
 var backValue = nvalue.substring(i+1);
 switch(i)
 {  
  case 1:if(parseInt(nvalue.substring(2),10) >59)
   obj.value = ovalue;
   if(parseInt(nvalue) > 3)
   obj.value = "0"+nvalue;
   break;
  case 2:var pvalue = nvalue;
  			pvalue = pvalue.replace(":","");
   if(parseInt(pvalue,10) >360)
   obj.value = ovalue;
   else if(parseInt(backValue,10)>5)
   	{
   		obj.value = nvalue.replace(":",":0");
   	}
   break;
  default:obj.value = ovalue;
  break;
 }
}

function check3value(obj,ovalue,nvalue)
{
 var i = nvalue.indexOf(":");
 switch(i)
 {	
  case 1:if(window.event.keyCode > 51)
  obj.value = "0" + nvalue;
  	break;
  case 2:if(parseInt(nvalue,10) >36)
   obj.value = ovalue;
   break;
  default:obj.value = ovalue;
  break;
 }
}
var oldvalue;
function keydown(obj)
{ 
    var kindex = window.event.keyCode;
    if(kindex == 8 || kindex ==46)
    oldvalue = obj.value;
}
function keyup(obj)
{
  var kindex = window.event.keyCode;
 if(kindex == 8 || kindex ==46)
 {
    if(oldvalue.indexOf(":") > -1 && obj.value.indexOf(":") == -1 && obj.value != "")
    {
        obj.value = oldvalue;
    }  
 }
}

var oldvalue;
//function keydown(obj)
//{ 
//    var kindex = window.event.keyCode;
//    if(kindex == 8 || kindex ==46)
//    oldvalue = obj.value;
//}
//function keyup(obj)
//{
//  var kindex = window.event.keyCode;
// if(kindex == 8 || kindex ==46)
// {
//    if(oldvalue.indexOf(":") > -1 && obj.value.indexOf(":") == -1 && obj.value != "")
//    {
//        obj.value = oldvalue;
//    }  
// }
//}
function Export()
{
///    alert("export");
    var search = window.location.search;
    var frm=document.forms[0];
    if(search.length>0) search = search.substring(1);
    var url = window.location.pathname + "?export=" + COMMAND_EXPORT + "&" + search;
    //alert(url);
    setCommand(COMMAND_EXPORT); 
    frm.submit();
////    window.nevigate(url);
}


function chekState()
{    
    if (document.readyState!="complete")
    {
        window.setTimeout("chekState()", 10);
    }
    else
    {
        if(typeof(firstrun)!="undefined")
        {
            firstrun();
        }
        else
        {
            if(typeof(selectFirst)!="undefined")
            {
                selectFirst();
            }
        }
       
    }
}
    	
function ActiveOut(e)
{
    
    e.value=ChangeCurrency(e.value,"");
    if (typeof(selfActiveOut) !="undefined")
    {
        selfActiveOut(e);   
    }
}


/*=====Report JavaScript ===begin==*/
/*
  MoveDateToConditionGrid() 
  RemoveToFilterGrid()
  AddRowStyles()
  AddCditRowStyles()
*/
        function MoveDateToConditionGrid(pFilter,pFilterValue,sDataGrid,sCellsA,sCellsB)
        {
            var sRow,sRowCdtCount;
            var sHadFound =true; //是否能找到
            var sAndOr = true; // 是否 用And 或 Or 来连接
            var reg=/(')/g;
            
//            sCellsA = sSourceRow.srcElement.parentElement.cells[0].innerText;
//            sCellsB = sSourceRow.srcElement.parentElement.cells[1].innerText;
         sRowCdtCount=  gvDataCondition.GridView.rows.length;
        //Filter ,Value,Code, SQLstr
        //循环检测DataGrid是否有重复的值
        //Todo:  
            if  (sRowCdtCount ==1)        //只有一条记录（column ）    
            {   
                sHadFound= false;
                sAndOr = true;
            }
            if (sRowCdtCount >1 )
            {
                if (gvDataCondition.GridView.rows(sRowCdtCount-1).cells(0).innerText== pFilter)
                {
               
                 sAndOr = false;   
                }
            }
            for (var i=1; i< sRowCdtCount ; i++)
            { 
              //Use Staff ID(cells(2)) Filter ,Name is cells(1)
              if ( gvDataCondition.GridView.rows(i).cells(2).innerText == sCellsA)
                 {  
                     sHadFound =true;
                     break;
                 }
                 else
                 {
                     sHadFound =false;
                 }  
            }                  
           
           if (sHadFound==false)
           {
           sRow= sDataGrid.GridView.insertRow();
           sRow.attachEvent("ondblclick",RemoveToFilterGrid); 
           sRow.detachEvent("onclick",AddCditRowStyles);  
           sRow.attachEvent("onclick",AddCditRowStyles);
           
           sRow.insertCell(0).innerText=pFilter;
           sRow.cells(0).className=srowCellClassName;  
           
           sRow.insertCell(1).innerText=sCellsB;
           sRow.cells(1).className=srowCellClassName;  

           sRow.insertCell(2).innerText=sCellsA;
           sRow.cells(2).className=srowCellClassName;
           //sRow.cells(2).runtimeStyle.display ="none";
           
//           if (sAndOr)
//           {
//               sRow.insertCell(3).innerText=" )and( " + pFilterValue +" = '" + sCellsA +"'";
//           }else
//           {
//               sRow.insertCell(3).innerText="   or  " + pFilterValue +" = '" + sCellsA +"'"; 
//           }
           // 2007-02-06 fix ' bug: 2'2'
           sRow.insertCell(3).innerText= pFilterValue +" = '" + sCellsA.replace(reg,"''") +"'";
           sRow.cells(3).className=srowCellClassName;
           
           sRow.cells(2).runtimeStyle.display ="none";  
           sRow.cells(3).runtimeStyle.display ="none";
             
           
           }
            
            
        }
        
        
        function RemoveToFilterGrid(evv)
        {
       
//         alert(event.srcElement.parentElement.cells[0].innerText);
//          alert(event.srcElement.parentElement.cells[1].innerText);
//         alert(event.srcElement.parentElement.cells[2].innerText);
//          alert(event.srcElement.parentElement.cells[3].innerText);

              var sFtr,sObjA,sObjB, sRowIndex,sRowFitCount;
              var sHadFound =true; //是否能找到
              
              //Filter ,Value,Code, SQLstr 
              sFtr = evv.srcElement.parentElement.cells[0].innerText; 
              sObjA = evv.srcElement.parentElement.cells[2].innerText; //code
              sObjB = evv.srcElement.parentElement.cells[1].innerText; //Name
              
              sRowFitCount = gvDataFilter.GridView.rows.length;
              if (sGlogbalFilter==sFtr) 
              {
              
                //循环检测DataGrid是否有重复的值
                 
                 if  (sRowFitCount ==1)        //只有一条记录（column ）    
                    {   
                        sHadFound= false;
                    }
                    for (var j=1; j< sRowFitCount ; j++)
                    { 
                      if ( gvDataFilter.GridView.rows(j).cells(0).innerText == sObjA)
                         {  
                             sHadFound =true;
                             break;
                         }
                         else
                         {
                             sHadFound =false;
                         }  
                    }                  


              }  
               if (sHadFound==false)
               {
                   sRow=  gvDataFilter.GridView.insertRow();
                   sRow.attachEvent("ondblclick",RemoveToCondition); 
                   sRow.detachEvent("onclick",AddRowStyles);  
                   sRow.attachEvent("onclick",AddRowStyles);
                   sRow.insertCell(0).innerText=sObjA;
                   sRow.cells(0).className=srowCellClassName;         
                   sRow.insertCell(1).innerText=sObjB;
                   sRow.cells(1).className=srowCellClassName;   
//                   //get new RowFitCount
//                   sRowFitCount = gvDataFilter.GridView.rows.length;                
               }
              
              
              //Remove Row
               sRowIndex = evv.srcElement.parentElement.rowIndex; //evv.srcElement.parentElement.selectRowIndex; 
               gvDataCondition.GridView.deleteRow(sRowIndex);     
            
        }
        
        
        
        function AddRowStyles(evv)
        {
          
           if ((gvDataFilter.GridView.rows.length-1) >= sSelectedRowIndex)
           {
             gvDataFilter.GridView.rows(sSelectedRowIndex).className ="";
           }
            evv.srcElement.parentElement.className=srowClassName;
            sSelectedRowIndex=evv.srcElement.parentElement.rowIndex;
        }

        function AddCditRowStyles(evv)
        {
          
           if ((gvDataCondition.GridView.rows.length-1) >= sCditSelectedRowIndex)
           {
             gvDataCondition.GridView.rows(sCditSelectedRowIndex).className ="";
           }
            evv.srcElement.parentElement.className=srowClassName;
            sCditSelectedRowIndex=evv.srcElement.parentElement.rowIndex;
        }


    	function getNewValue(oValue)
    	{
    	    var nValue="";
    	    nValue = oValue.substr(oValue.indexOf('&$&')+3);
    	    return nValue;
    	}
    	function getAndOr(oValue)
    	{
    	   var nValue="";
    	    nValue = oValue.substr(0,oValue.indexOf('&$&'));
    	    return nValue;
    	}
    	 
/*=====Report JavaScript ===end==*/

//       function ShowDateTypeErrorYYYYMM(control)
//       {
//       
//           if(window.event.keyCode>=48 && window.event.keyCode<=57)
//           {
//              if(control.value.length==0 && window.event.keyCode < 49)
//              {
//                 window.event.keyCode="";
//              }
//              if(control.value.length==4)
//              {
//                 control.value=control.value + "-";
//              }
//              if(control.value.length > 4)
//              {
//                 control.value.replace(control.value.substring(4,5),"-");
//              }
//              if(control.value.length==5 && window.event.keyCode > 49)
//              {
//                  window.event.keyCode=48;
//              }
//              if(control.value.length==6)
//              {
//                  if(control.value.substring(5,6)-0==0)  //****-0*
//                  {
//                      if(window.event.keyCode < 49 || window.event.keyCode > 57)
//                      {
//                         window.event.keyCode="";
//                      }
//                  }
//                  else if(control.value.substring(5,6)-0==1)  //****-1*
//                  {
//                      if(window.event.keyCode < 47 || window.event.keyCode > 50)
//                      {
//                         window.event.keyCode="";
//                      }
//                  }
//              }
//              
//           }
//           else
//           {
//                window.event.keyCode="";
//           }
//       }
       
       //add by kamte at 2007-01-16 for YYYY-MM-DD
       var docRange;    //document.selection.createRange();
       var txtRange;    //obj.createTextRange();
       var mlength      //obj.maxLength;
       var dayArray = new Array(31,29,31,30,31,30,31,31,30,31,30,31);
       function checktextIndex(obj) //确认输入的数字在文本中的位置
       {            
                mlength = obj.maxLength;
                obj.maxLength = 100;
                docRange = document.selection.createRange();
                docRange.text = "m";
            var lIndex = obj.value.indexOf("m");            
                txtRange = obj.createTextRange();
                txtRange.findText("m",0);
                txtRange.select();
                docRange = document.selection.createRange();
                docRange.text = "";
                docRange.select();
                obj.maxLength = mlength;
                return lIndex;
       }
       function finddivChar(ovalue,index) //确认分隔符‘－’的位置,index 1,为得第一个，2为得第二个，当返回－2时，说明一个也没有
       {
            var divIndex = ovalue.indexOf("-");
            if(divIndex == -1)
                return -2;
                if(index == 2)
                divIndex = ovalue.indexOf("-",divIndex+1);
            return divIndex;
       }
       function getnextChar(ovalue,index,next)  //获取index的下一字符
       {
            index = index + next;
            return ovalue.substr(index,1);
       }
       function getmaxDay(ovalue)   //返回一个月能拥有的最大天数
       {
            var year = getinputDate(ovalue,1);
            var month = getinputDate(ovalue,2);
            
            if(year.length != 4 || month.length != 2)
                return 31;
            if(month == "")
                return 31;
            else
            {
                if(year == "")
                    return dayArray[parseInt(month,10)-1];
                else
                {
                    if((parseInt(month,10) == 2) && ((parseInt(year,10) % 4)  > 0))
                    {
                        return 28;                    
                    }
                    
                    return dayArray[parseInt(month,10)-1];
                }
            }
            
            return 31;
       }
       function getinputDate(ovalue,type) //返回输入的天数
       {
            var count = "";
            switch(type)
            {
                case 1:count = ovalue.substring(0,finddivChar(ovalue,0));//year
                    break;
                case 2:count = ovalue.substring(finddivChar(ovalue,0)+1,finddivChar(ovalue,2));//month
                    break;
                case 3:count = ovalue.substr(finddivChar(ovalue,2)+1);//day
                    break;
            } 
            return count;
       }
       function getsecmaxDayofyear(year,month)
       {
            var maxday = 31;
                maxday = dayArray[parseInt(month,10)-1];
            
            if(year.length != 4)
                return maxday;
            if(parseInt(month,10) == 2 && parseInt(year,10) % 4 != 0)
                return 28;
                
                return maxday;            
       }
       
       function setPressCode(obj,kIndex,lIndex,divIndex)    //对按键事件进行处理
       {   
           
            if(divIndex == -2)  //没有一个分隔符
            {
                if(kIndex == 45) 
                {
                    window.event.keyCode = 0;
                    return;
                }
                if(obj.value.length == 3)
                {
                    docRange = document.selection.createRange();
                    docRange.text = String.fromCharCode(kIndex);
                    obj.value += "-";
                    window.event.keyCode = 0;
                    return;               
                }
            }
            else    //有第一个分隔符
            {
                if(lIndex<=divIndex) //当输入位置小于第一个分隔符
                  {
                    if(kIndex == 45) 
                    {
                        window.event.keyCode = 0;
                        return;
                    }
                    if(divIndex == 4)
                    {
                        window.event.keyCode = 0;
                        return;
                    }
                    var nextIndex = finddivChar(obj.value,2);
                    if(nextIndex != -1)
                    {
                        var year = getinputDate(obj.value,1);
                        var month = getinputDate(obj.value,2);
                        if(year.length == 3 && month == 2)
                        {
                            var day = getinputDate(obj.value,3);
                            if(day != "" && day == 29)
                            {
                               mlength = obj.maxLength;
                                obj.maxLength = 100;
                                docRange = document.selection.createRange();
                                docRange.text = String.fromCharCode(kIndex)+"h";
                                year = getinputDate(obj.value,1).replace("h","");
                               if(parseInt(getsecmaxDayofyear(year,"02")) != 29)
                               {
                                    obj.value = obj.value.substring(0,finddivChar(obj.value,2)+1) + "28";
                               }
                                txtRange = obj.createTextRange();
                                txtRange.findText("h",0);
                                txtRange.select();
                                docRange = document.selection.createRange();
                                docRange.text = "";
                                docRange.select();
                                obj.maxLength = mlength;
                                window.event.keyCode = 0;
                                
                                return;  
                            }                            
                        }     
                    }
                  }
                  else  //当输入位置大于第一个分隔符
                  {
                        var nextIndex = finddivChar(obj.value,2);  //得第二个分隔符的位置
                        if(nextIndex == -1) //没有第二个分隔符
                        {                            
                            //确认月份区间为01-12
                            if(lIndex == (divIndex+1))  //处理月份第一位
                            {
                                if(kIndex == 45) 
                                {
                                    window.event.keyCode = 0;
                                    return;
                                }
                                var nextChar = getnextChar(obj.value,lIndex,0);
                                if(nextChar != "")
                                {
                                    if(nextChar == "0")
                                    {
                                        if(kIndex != 49)
                                        {
                                            window.event.keyCode = 0;
                                            return;
                                        }    
                                    }
                                    else
                                    {
                                        if(kIndex >= 50)
                                        {
                                            window.event.keyCode = 0;
                                            return;
                                        }
                                    }       
                                }
                                else
                                {
                                    if(kIndex >= 50)
                                    {
                                        if(obj.maxLength == 7)
                                        {
                                            obj.value += "0" + String.fromCharCode(kIndex);
                                        }
                                        else
                                        {
                                            obj.value += "0" + String.fromCharCode(kIndex) + "-";
                                        }
                                        window.event.keyCode = 0;
                                        return;  
                                    }
                                }
                            }
                            else    //处理月份的第二位
                            {
                                var preChar = getnextChar(obj.value,lIndex,-1);
                                if(kIndex == 45 ) 
                                {
                                    if( preChar.toString()=="1")
                                    {
                                        obj.value = obj.value.substring(0,divIndex+1) + String.fromCharCode(48) + preChar + "-";
                                    }                                   
                                    window.event.keyCode = 0;
                                    return;
                                }
                                
                                if(preChar == "0")
                                {
                                    if(kIndex == 48)
                                    {
                                        window.event.keyCode = 0;
                                        return;
                                    }    
                                }
                                else
                                {
                                    if(kIndex >= 51)
                                    {
                                         window.event.keyCode = 0;
                                         return;
                                    }
                                }
                            }
                            if((divIndex + 2) == (obj.value.length))
                            {
                                docRange = document.selection.createRange();
                                docRange.text = String.fromCharCode(kIndex);
                                if(obj.maxLength == 10)
                                obj.value += "-";
                                window.event.keyCode = 0;
                                return;  
                            }
                        }
                        else //有第二个分隔符
                        {
                            if(kIndex == 45) 
                            {
                                window.event.keyCode = 0;
                                return;
                            }
                            if(lIndex <= nextIndex)  //输入位置在第二个分隔符之前，第一个后
                            {
                                if((divIndex + 3) == nextIndex) //月份最多只能有两位
                                {
                                    window.event.keyCode = 0;
                                    return;
                                }
                                //确认月份区间为01-12
                                var preChar = getnextChar(obj.value,lIndex,-1);
                                var nextChar = getnextChar(obj.value,lIndex,0);
                                if(lIndex == (divIndex+1))  //处理月份第一位
                                {                                   
                                    switch(nextChar)
                                    {
                                        case "-":
                                            if(kIndex >= 50)
                                            {
                                                var imaxDay = getsecmaxDayofyear(getinputDate(obj.value,1),String.fromCharCode(kIndex));                                    
                                                if( imaxDay < parseInt(getinputDate(obj.value,3),10))
                                                {
                                                    mlength = obj.maxLength;
                                                    obj.maxLength = 100;
                                                    docRange = document.selection.createRange();
                                                    docRange.text = "0"+ String.fromCharCode(kIndex)+"o";
                                                    obj.value = obj.value.substring(0,finddivChar(obj.value,2)+1) + imaxDay.toString();
                                                    txtRange = obj.createTextRange();
                                                    txtRange.findText("o",0);
                                                    txtRange.select();
                                                    docRange = document.selection.createRange();
                                                    docRange.text = "";
                                                    docRange.select();
                                                    obj.maxLength = mlength;
                                                    window.event.keyCode = 0;
                                                    
                                                    return;
                                                }
                                                else
                                                {                                                
                                                     docRange = document.selection.createRange();
                                                     docRange.text = "0" + String.fromCharCode(kIndex);                                    
                                                     docRange.select();
                                                     window.event.keyCode = 0;
                                                     return;
                                                }
                                            }
                                            break;
                                        case "0":
                                            if(kIndex != 49)
                                            {
                                                window.event.keyCode = 0;
                                                return;
                                            }                                            
                                            break;
                                        case "1":
                                            if(kIndex >= 50)
                                            {
                                                window.event.keyCode = 0;
                                                return;
                                            }
                                            else
                                            {
                                                if(kIndex == 49 && getinputDate(obj.value,3) != "" && getinputDate(obj.value,3).toString() == "31")
                                                {
                                                    mlength = obj.maxLength;
                                                    obj.maxLength = 100;
                                                    docRange = document.selection.createRange();
                                                    docRange.text = String.fromCharCode(kIndex)+"o";
                                                    obj.value = obj.value.substring(0,(obj.value.length-1)) + "0";
                                                    txtRange = obj.createTextRange();
                                                    txtRange.findText("o",0);
                                                    txtRange.select();
                                                    docRange = document.selection.createRange();
                                                    docRange.text = "";
                                                    docRange.select();
                                                    obj.maxLength = mlength;
                                                    window.event.keyCode = 0;
                                                        
                                                        return;
                                                }
                                            }
                                            break;
                                        case "2":
                                            if(kIndex >= 50)
                                            {
                                                window.event.keyCode = 0;
                                                return;
                                            }
                                            else
                                            {
                                                if(kIndex == 48 && getinputDate(obj.value,3) != "" && parseInt(getinputDate(obj.value,3),10) > 28)
                                                {
                                                    var imaxDay = getsecmaxDayofyear(getinputDate(obj.value,1),"02");                                    
                                                    if( imaxDay < parseInt(getinputDate(obj.value,3),10))
                                                    {
                                                        mlength = obj.maxLength;
                                                        obj.maxLength = 100;
                                                        docRange = document.selection.createRange();
                                                        docRange.text = String.fromCharCode(kIndex)+"o";
                                                        obj.value = obj.value.substring(0,finddivChar(obj.value,2)+1) + imaxDay.toString();
                                                        txtRange = obj.createTextRange();
                                                        txtRange.findText("o",0);
                                                        txtRange.select();
                                                        docRange = document.selection.createRange();
                                                        docRange.text = "";
                                                        docRange.select();
                                                        obj.maxLength = mlength;
                                                        window.event.keyCode = 0;
                                                        
                                                        return;
                                                    }
                                                }
                                            }
                                            break;
                                        default:
                                            if(kIndex != 48)
                                            {
                                                window.event.keyCode = 0;
                                                return;
                                            }
                                            break;
                                        
                                    }   
                                }
                                else    //处理月份的第二位
                                {
                                    switch(preChar)
                                    {
                                        case "0":
                                            if(kIndex == 48)
                                            {
                                                window.event.keyCode = 0;
                                                return;
                                            }
                                            else
                                            {
                                                var imaxDay = getsecmaxDayofyear(getinputDate(obj.value,1),String.fromCharCode(kIndex));                                    
                                                    if( imaxDay < parseInt(getinputDate(obj.value,3),10))
                                                    {
                                                        mlength = obj.maxLength;
                                                        obj.maxLength = 100;
                                                        docRange = document.selection.createRange();
                                                        docRange.text = String.fromCharCode(kIndex)+"o";
                                                        obj.value = obj.value.substring(0,finddivChar(obj.value,2)+1) + imaxDay.toString();
                                                        txtRange = obj.createTextRange();
                                                        txtRange.findText("o",0);
                                                        txtRange.select();
                                                        docRange = document.selection.createRange();
                                                        docRange.text = "";
                                                        docRange.select();
                                                        obj.maxLength = mlength;
                                                        window.event.keyCode = 0;
                                                        
                                                        return;
                                                    }
                                            }
                                            break;
                                        case "1":
                                            if(kIndex >= 51)
                                            {
                                                window.event.keyCode = 0;
                                                return;
                                            }
                                            else
                                            {
                                                if(kIndex == 49 && getinputDate(obj.value,3) != "" && getinputDate(obj.value,3).toString() == "31")
                                                {
                                                    mlength = obj.maxLength;
                                                    obj.maxLength = 100;
                                                    docRange = document.selection.createRange();
                                                    docRange.text = String.fromCharCode(kIndex)+"o";
                                                    obj.value = obj.value.substring(0,(obj.value.length-1)) + "0";
                                                    txtRange = obj.createTextRange();
                                                    txtRange.findText("o",0);
                                                    txtRange.select();
                                                    docRange = document.selection.createRange();
                                                    docRange.text = "";
                                                    docRange.select();
                                                    obj.maxLength = mlength;
                                                    window.event.keyCode = 0;                                                        
                                                    return;
                                                }
                                            }
                                            break;
                                        default:
                                            window.event.keyCode = 0;
                                            break;
                                    }
                                }
                                   
                            }
                            else  //在第二个分隔符之后,日期的处理
                            {
                                if(kIndex == 45) 
                                {
                                    window.event.keyCode = 0;
                                    return;
                                }
                                if((nextIndex + 1 + 2) == obj.value.length)
                                {
                                    window.event.keyCode = 0;
                                    return;
                                }
                                //日期的处理
                                var preChar = getnextChar(obj.value,lIndex,-1);
                                var nextChar = getnextChar(obj.value,lIndex,0);
                                var maxDay = getmaxDay(obj.value);
                                var newday = "0";
                                if(lIndex == nextIndex + 1) //处理天数的第一位
                                {
                                    if(nextChar == "")
                                    {
                                         newday = String.fromCharCode(kIndex) + "0";
                                         if(parseInt(newday,10) > maxDay)
                                         {
                                            docRange = document.selection.createRange();
                                            docRange.text = "0" + String.fromCharCode(kIndex);                                    
                                            docRange.select();
                                            window.event.keyCode = 0;
                                            return;
                                         }
                                    }
                                    else    //有天数的第二位
                                    {
                                        newday = String.fromCharCode(kIndex) + nextChar;
                                        if(parseInt(newday,10) > maxDay)
                                         {                                            
                                            window.event.keyCode = 0;
                                            return;
                                         }
                                    }
                                }
                                else //处理天数的第二位
                                {
                                    newday = preChar + String.fromCharCode(kIndex);
                                    if(parseInt(newday,10) > maxDay)
                                     {                                            
                                        window.event.keyCode = 0;
                                        return;
                                     }
                                }
                            }
                        }
                  }
            }
       }
       function Datekeypress(obj)
        {
            var kindex = window.event.keyCode;
             if(!((kindex >= 48 && kindex <= 57)|| kindex==45))
             {
                 window.event.keyCode = 0;
                 return;
             }
             
             if(obj.value.length == 10)
             {
                window.event.keyCode = 0;
                return;
             }
             
             var lIndex = -1;
             var divIndex = finddivChar(obj.value,1);
             if(obj.value.length <10)
                 lIndex = checktextIndex(obj);    //确认输入的数字在文本中的位置
                        
             setPressCode(obj,kindex,lIndex,divIndex);
        }
        var keyArray = new Array(8,9,109,189,46,37,39,13,16,17,33,34,35,36,38,40,45);
        function containkeyCode(kIndex)
        {
            var contain = false;
            for(var i=0;i<keyArray.length;i++)
            {
                if(keyArray[i] == kIndex)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        function Datekeydown(obj)
        { 
            if(obj.readOnly)
            {
                window.event.returnValue = false;
                return;
             } 
            var kindex = window.event.keyCode;
            if(containkeyCode(kindex))
            {
                docRange = document.selection.createRange();
                if(docRange.text != "" && docRange.text != obj.value)
                {
                    window.event.returnValue = false;
                    return;
                }
                var divIndex = finddivChar(obj.value,1);
                if(divIndex >= 0)
                {
                    var lIndex = 0;
                    if(docRange.text == "")
                    {
                        lIndex = checktextIndex(obj);                   
                    } 
                    var nextIndex = finddivChar(obj.value,2);
                    switch(kindex)
                    {
                        case 8: //BackSpace
                            if(((divIndex + 1) == lIndex) || (nextIndex > 0 && (nextIndex+1 == lIndex)))
                            {
                               window.event.returnValue = false;
                            }
                            break;                       
                        case 46:    //Delete
                            if((divIndex == lIndex) || (nextIndex >0 && nextIndex == lIndex))
                            {
                               window.event.returnValue = false;
                            }
                            break;
                        case 9: //Tab 
                             if (typeof(DateKeyDownSelf) != "undefined")
                             {
                                DateKeyDownSelf();
                             }
                            break;    
                        default:break;
                    }
                }    
             }
             else
             { 
                if(!((kindex >= 48 && kindex <=57) || (kindex >=96 && kindex <=105))) 
                {
                    window.event.returnValue = false;
                    return;
                 }
                
             }
            
        }              
       //end YYYY-MM-DD
   
   //add by kamte at 2007-01-18,HH:MM   
   function timefinddivChar(ovalue) //确认分隔符':'的位置
   {
        var divIndex = ovalue.indexOf(":");        
        return divIndex;
   }
   function setTimePressCode(obj,kIndex,lIndex,divIndex)
   {
        if(divIndex == -1)  //没有分隔符
        {
            var preChar = getnextChar(obj.value,lIndex,-1);
            var nextChar = getnextChar(obj.value,lIndex,0);
            if(lIndex == 0) //小时的第一位
            {
                if(nextChar == "") // 没有第二位
                {
                    if(kIndex >= 52) //最大 36:00
                    {
                        docRange = document.selection.createRange();
                        docRange.text = String.fromCharCode(48) + String.fromCharCode(kIndex) + ":";
                        docRange.select();
                        window.event.keyCode = 0;
                        return;
                    }
                }
                else //有第二位
                {
                    if(kIndex >= 52)
                    {
                        window.event.keyCode = 0;
                        return;
                    }
                    
                    if((parseInt(nextChar,10) > 6) && kIndex >= 51)
                    {
                        window.event.keyCode = 0;
                        return;
                    }
                }                
            }
            else   //小时的第二位
            {
                if(preChar.toString() == "3" && kIndex >= 55)
                {
                    window.event.keyCode = 0;
                    return;
                }
                else
                {
                    docRange = document.selection.createRange();
                    docRange.text = String.fromCharCode(kIndex) + ":";
                    docRange.select();
                    window.event.keyCode = 0;
                    return;
                } 
            }
        }
        else    //有分隔符
        {
            if(lIndex <= divIndex)  //在分隔符之前，处理小时
            {
                if(divIndex == 2)
                {
                    window.event.keyCode = 0;
                    return;
                }
                var minutes = getHoursorMinutes(obj.value,divIndex,2);
                var preChar = getnextChar(obj.value,lIndex,-1);
                var nextChar = getnextChar(obj.value,lIndex,0);
                if(lIndex == 0) //小时的第一位
                {
                    switch(nextChar.toString())
                    {
                        case ":":
                            if(kIndex >= 52)
                            {
                                docRange = document.selection.createRange();
                                docRange.text = String.fromCharCode(48) + String.fromCharCode(kIndex);
                                docRange.select();
                                window.event.keyCode = 0;
                                return;
                            }
                            break;
                        case "6":
                            if(kIndex >= 52)
                            {
                                window.event.keyCode = 0;
                                return;
                            }
                            if(kIndex == 51)
                            {
                                mlength = obj.maxLength;
                                obj.maxLength = 100;
                                docRange = document.selection.createRange();
                                docRange.text = String.fromCharCode(kIndex) + "m";
                                divIndex = timefinddivChar(obj.value); 
                                obj.value = obj.value.substring(0,divIndex+1) + "00";
                                txtRange = obj.createTextRange();
                                txtRange.findText("m",0);
                                txtRange.select();
                                docRange = document.selection.createRange();
                                docRange.text = "";
                                docRange.select();
                                obj.maxLength = mlength;
                                window.event.keyCode = 0;
                                return;                                
                            }
                            break;
                        default:
                            if(parseInt(nextChar,10) > 6)
                            {
                                if(kIndex >= 51)
                                {
                                    window.event.keyCode = 0;
                                    return;
                                }
                            }
                            else
                            {
                                if(kIndex >= 52)
                                {
                                    window.event.keyCode = 0;
                                    return;
                                }
                            }
                            break;
                    }
                }
                else //小时的第二位
                {
                    switch(preChar.toString())
                    {
                        case "3":
                            if(kIndex >= 55)
                            {
                                window.event.keyCode = 0;
                                return;
                            }
                            else if(kIndex == 54)
                            {
                                mlength = obj.maxLength;
                                obj.maxLength = 100;
                                docRange = document.selection.createRange();
                                docRange.text = String.fromCharCode(kIndex) + "m";
                                divIndex = timefinddivChar(obj.value);
                                obj.value = obj.value.substring(0,divIndex+1) + "00";
                                txtRange = obj.createTextRange();
                                txtRange.findText("m",0);
                                txtRange.select();
                                docRange = document.selection.createRange();
                                docRange.text = "";
                                docRange.select();
                                obj.maxLength = mlength;
                                window.event.keyCode = 0;
                                return;            
                            }
                            break;
                        default:
                            if(parseInt(preChar,10) >= 4)
                            {
                                window.event.keyCode = 0;
                                return;
                            }
                            break;
                    }
                }
            }
            else    //在分隔符之后，处理分钟
            {
                if((divIndex + 1 + 2) == obj.value.length)
                {
                    window.event.keyCode = 0;
                    return;
                }
                var hours = getHoursorMinutes(obj.value,divIndex,1);
                if(lIndex == (divIndex + 1)) //分钟的第一位
                {
                    if(hours == 36)
                    {
                        if(kIndex != 48)
                        {
                            window.event.keyCode = 0;
                            return;
                        }
                    }
                    else
                    {
                        if(kIndex >= 54)
                        {
                            docRange = document.selection.createRange();
                            docRange.text = String.fromCharCode(48) + String.fromCharCode(kIndex);
                            docRange.select();
                            window.event.keyCode = 0;
                            return;
                        }
                    }
                }
                else //分钟的第二位
                {
                    if(hours == 36 && kIndex != 48)
                    {
                        window.event.keyCode = 0;
                        return;
                    }
                }
            }
        }
   }
   function getHoursorMinutes(ovalue,divIndex,type) //当有分隔符才可以调用
   {
        var count = "";
        if(type==1) //小时
        {
            count = ovalue.substring(0,divIndex);
        }
        else
        {
            count = ovalue.substr(divIndex+1);
        }
        if(count == "") count = 0;
        
        return parseInt(count,10);
   }
   function Timekeypress(obj)
   {
        var kindex = window.event.keyCode;
         if(kindex < 48 || kindex >57)
         {
             window.event.keyCode = 0;
             return;
         }
         
         if(obj.value.length == 5)
         {
            window.event.keyCode = 0;
            return;
         }
         
        var lIndex = -1;
        var divIndex = timefinddivChar(obj.value);       
            lIndex = checktextIndex(obj);    //确认输入的数字在文本中的位置                     
         
         setTimePressCode(obj,kindex,lIndex,divIndex);
   }
   function Timekeydown(obj)
   {  
            if(obj.readOnly)
            {
                window.event.returnValue = false;
                return;
             } 
        var kindex = window.event.keyCode;
        if(containkeyCode(kindex))
        {
            if(kindex == 8 || kindex ==46)
            {
                docRange = document.selection.createRange();
                if(docRange.text != "" && docRange.text != obj.value)
                {
                    window.event.returnValue = false;
                    return;
                }
                var divIndex = timefinddivChar(obj.value);
                if(divIndex >= 0)
                {
                    var lIndex = 0;
                    if(docRange.text == "")
                    {
                     lIndex = checktextIndex(obj);                                    
                    } 
                    switch(kindex)
                    {
                        case 8: //BackSpace
                            if((divIndex + 1) == lIndex)
                            {
                               window.event.returnValue = false;
                            }
                            break;
                        case 46:    //Delete
                            if(divIndex == lIndex)
                            {
                               window.event.returnValue = false;
                            }
                            break;
                        default:break;
                    }
                }    
             }
          }   
         else
         {
            if((kindex >= 48 && kindex <=57) || (kindex >=96 && kindex <=105))
            {}
            else
            {
                window.event.returnValue = false;
                return;
            }
         }
   }
   //end HH:MM
   
   //add by kamte at 200701-22 
   //Begin Currency
   function CurrencyfinddivChar(ovalue,vchar) //确认分隔符'.'的位置
   {
        var divIndex = ovalue.indexOf(vchar);        
        return divIndex;
   }
   function setCurrencyPressCode(obj,kindex,lIndex,divIndex)
   {
        
        var preChar = getnextChar(obj.value,lIndex,-1);
        var nextChar = getnextChar(obj.value,lIndex,0);
        
        if(lIndex == 0 && nextChar == ".")
        {
            mlength = obj.maxLength;
            obj.maxLength = 9100;
            docRange = document.selection.createRange();
            docRange.text = String.fromCharCode(kindex) +String.fromCharCode(48) + "m";
            txtRange = obj.createTextRange();
            txtRange.findText("m",0);
            txtRange.select();
            docRange = document.selection.createRange();
            docRange.text = "";
            docRange.select();
            obj.maxLength = mlength;
            window.event.keyCode = 0;
            return;
        }
        if(kindex == 48 && lIndex == 0)
        {
            
            if(divIndex > -1)
            {
                var reg = /(\.|,)/g;
                obj.value = obj.value.replace(reg,"");
                obj.value = String.fromCharCode(kindex) +String.fromCharCode(46) + obj.value;
                window.event.keyCode = 0;
                return;
            }
            else
            {
                 mlength = obj.maxLength;
                obj.maxLength = 9100;
                docRange = document.selection.createRange();
                docRange.text = String.fromCharCode(kindex) +String.fromCharCode(46) + "m";
                txtRange = obj.createTextRange();
                txtRange.findText("m",0);
                txtRange.select();
                docRange = document.selection.createRange();
                docRange.text = "";
                docRange.select();
                obj.maxLength = mlength;
                window.event.keyCode = 0;
                return;
            }
        }        
       
        if((preChar == "-" && kindex==46) || (kindex==46 && lIndex==0))
        {
            mlength = obj.maxLength;
            obj.maxLength = 9100;
            docRange = document.selection.createRange();
            docRange.text = String.fromCharCode(48) +String.fromCharCode(kindex) + "m";
            txtRange = obj.createTextRange();
            txtRange.findText("m",0);
            txtRange.select();
            docRange = document.selection.createRange();
            docRange.text = "";
            docRange.select();
            obj.maxLength = mlength;
            window.event.keyCode = 0;
            return;        
        }
        
   }
   function Currencykeypress(obj)
   {
        
        var kindex = window.event.keyCode;
         
         if((kindex>=48 && kindex<=57) || kindex==45 || kindex ==46 || kindex == 40 || kindex == 41)         
         {}else
         {
             window.event.keyCode = 0;
             return;
         }
         
        var lIndex = -1;        
            lIndex = checktextIndex(obj);    //确认输入的数字在文本中的位置                     
            //alert(lIndex);return;
        var val = obj.value;
        if(lIndex != 0 && kindex == 40)                
        {
            window.event.keyCode = 0;
            return;
        }
        if (kindex==41)
        {
            if (lIndex != val.length)
            {
                window.event.keyCode = 0;
                return;
            }
            if (val.substring(0,1) != "(")
            {
                window.event.keyCode = 0;
                return;
            }
            if (val.indexOf(")") >= 0)
            {
                window.event.keyCode = 0;
                return;
            }
        }
        if (kindex>=48 && kindex<=57 )
        {
            
            if (lIndex > val.indexOf("("))
            {
                // Check ")" Exists 
                
                if (val.indexOf(")") >=0 && lIndex <= val.indexOf(")"))
                {
                } else 
                {
                    if (val.indexOf(")") >= 0) 
                    {
                        
                        window.event.keyCode = 0;
                        return; 
                    }
                }
            } else
            {
            
            }
        }
       
       if(lIndex != 0 && kindex == 45)  //负号只能出现在第一位
       {
            window.event.keyCode = 0;
            return;
       }
       if(obj.value.length > 0)
       {
            var nextChar = getnextChar(obj.value,lIndex,0);
            if(lIndex == 0 && nextChar == "-")
            {
                window.event.keyCode = 0;
                return;
            }
        
       }
       
       var divIndex = CurrencyfinddivChar(obj.value,".");
       if(divIndex != -1 && kindex == 46)
       {
            window.event.keyCode = 0;
            return;
       }
       
       setCurrencyPressCode(obj,kindex,lIndex,divIndex);
   }
  
   function Currencykeydown(obj)
   {
        if(obj.readOnly)
            {
                window.event.returnValue = false;
                return;
             } 
        var kindex = window.event.keyCode;
          if(containkeyCode(kindex))
            {}
             else
             { 
                if(!((kindex >= 48 && kindex <=57) || (kindex >=96 && kindex <=105)|| kindex==110 || kindex==190 || kindex == 40 || kindex == 41) ) 
                {
                    window.event.returnValue = false;
                    return;
                 }
             }

   }
   function Currencychange(obj)
   {
        
        CurrencyStandardSetting(obj);
   }
   function setstandardCurrency(num,isredu)
   {
        var newvalue = "";
        var numlength = num.toString().length;
        var ovalue = num.toString();
        if(isredu == 0)
        {
            numlength--;
            ovalue = ovalue.substr(1);
        }
        while(numlength > 3)
        {
            newvalue = ","+ovalue.substr(numlength-3,3) + newvalue;
            numlength = numlength - 3;
        }
        if(numlength > 0)
        {
            newvalue = ovalue.substr(0,numlength) + newvalue;
        }else
        {
            newvalue = ovalue;
        }
        if(isredu == 0)
            newvalue = "-"+newvalue;
            return newvalue;
   }
   function CurrencyStandardSetting(obj)
   {
        
        var reg=/(,)/g;
            obj.value = obj.value.replace(reg,"");
         var reduchar =  CurrencyfinddivChar(obj.value,"-");
         var lastdecimal = "";
         if(CurrencyfinddivChar(obj.value,".").toString() != "-1")
         {
            lastdecimal = obj.value.substring(CurrencyfinddivChar(obj.value,"."));
         }
         if(lastdecimal.length >3)  //保留两位小数
         {
            lastdecimal = lastdecimal.substring(1);
            var achar = lastdecimal.substr(2,1);
            lastdecimal = lastdecimal.substr(0,2);
            if(parseInt(achar,10)>=5)
            {
                lastdecimal = parseInt(lastdecimal,10) + 1;
            }
            lastdecimal = "." + lastdecimal.toString();
         }
         else
         {
            if(lastdecimal.length <= 1)
            {
                lastdecimal = ".00";
            }
            if(lastdecimal.length == 2)
            {
                lastdecimal = lastdecimal + "0";
            }                       
         }
         //var currchar = parseInt(obj.value,10);
         
         var currchar;
         if (obj.value.substring(0,1) = "(")
         {
            currchar = parseInt(obj.value.substring(1,obj.value.length),10);         
         } else
         {
            currchar = parseInt(obj.value,10);
         }         
         
         
         if(isNaN(currchar) && lastdecimal.length <= 1)
         {
            obj.value = "";
            return;
         }
         if(isNaN(currchar) && lastdecimal.length >1)
         {
            obj.value = "0"+lastdecimal;
            return;
         }         
        var snum = setstandardCurrency(currchar,reduchar);
        if(lastdecimal.length > 1)
            snum = snum + lastdecimal;
            
            //alert(snum);
        if (obj.value.substring(0,1) = "(")
        {
            obj.value = "(" + snum + ")";
        } else 
        {
            obj.value = snum;
        }
        
   }   
   //end Currency 
   
    CommandJs = function()
    {}       
    CommandJs.regDot = function()
    {
        //正则表达式 [,]
        return /(,)/g;
    }
    
    //.net's Trim
    CommandJs.ValidatorTrim= function(s)
    {
      var m = s.match(/^\s*(\S+(\s+\S+)*)\s*$/);
         return (m == null) ? "" : m[1];
    }
//    // Add a function called trim as a method of the prototype 
//    // object of the String constructor.
//    String.prototype.trim = function() {
//       // Use a regular expression to replace leading and trailing 
//       // spaces with the empty string
//       return this.replace(/(^\s*)|(\s*$)/g, "");
//    }
     
    //返回保留radixpoint的Float类型的值
    CommandJs.parseFloat = function(vv,radixpoint)
    {
        var _e=1;
        for (var i=1; i< radixpoint+1; i++)
        {
            _e*=10;
        }
        
        //remove "," character
        var reg=/(,)/g;
            vv =  vv.toString().replace(reg,"");
            
        return Math.round(parseFloat(vv)*_e) / _e;
    }
    
    //返回格式化后的float ,保留radixpoint格式的 string
    //NumberObj 还有个默认函数 ().toFixed(2) 也可以实现(可以使用这个)
    CommandJs.parseFloatStringFormat = function(vv,radixpoint)
    {
        var _e=1;
        for (var i=1; i< radixpoint+1; i++)
        {
            _e*=10;
        }
        //remove "," character
        var reg=/(,)/g;
            vv =  vv.toString().replace(reg,"");
                    
        var o = Math.round(parseFloat(vv)*_e) / _e;
        var xMatch = "";
        if (o.toString().indexOf(".",0) < 0 )
        {   
            for (var i=1;i<=radixpoint;i++)
            {
                xMatch += "0";
            }
            o = o + "." + xMatch;
        }
        else
        {
            //补位数0 ,补够radixpoint
            var pLength  = o.toString().length ;
            var pIndexof = o.toString().indexOf(".",0);
            if (pLength - pIndexof -1 < radixpoint)
            {
                for (var k= 1;k <= radixpoint-(pLength-pIndexof -1);k++)
                {
                   xMatch += "0";
                }
                o = o + xMatch;
            }
        }
        
      return  o.toString();
    }
    
    CommandJs.padLeft = function(sObj,sLength,sChart)
    {
       var sObjLength = sObj.length;
       if (sObj.length < sLength)
       {
            for (var j=0;j <sLength-sObjLength;j++)
            {
               sObj = sChart +sObj;
            }
       }
       return sObj;
    }
    CommandJs.padRight = function(sObj,sLength,sChart)
    {
       var sObjLength = sObj.length;
       if (sObj.length < sLength)
       {
            for (var j=0;j <sLength-sObjLength;j++)
            {
               sObj = sObj + sChart;
            }
       }
       return sObj;
    }
    
    CommandJs.ReSelectedCurrentGridRecord =function ()
    {
        //Fix search inbox box is not null
        if (txtIndex.value <= gvData.GridView.rows.length -1)
        {
            gvData.GridView.rows(txtIndex.value*1).onclick(); 
        }else
        {
            gvData.GridView.rows(1).onclick();
        }    
    }
    
    //if return == true ,mean occur error
    CommandJs.AjaxOnError = function (e)
    {
        if (typeof(AjaxPro) !="undefined" && AjaxPro.MyOnError != null)
        {
           return  AjaxPro.MyOnError(e);
        }
    }
    

    function NoEnter()
    {
        if(window.event.keyCode ==13)
        window.event.keyCode = 0;
    }
    
    function IntegerKeyPress(ctrl)
   {
        var keycode = window.event.keyCode;
         
         if((keycode>=48 && keycode<=57) || keycode==45 || keycode == 40 || keycode == 41)         
         {
         }
         else
         {
             window.event.keyCode = 0;
             return;
         }
         
        var lIndex = -1;        
            lIndex = checktextIndex(ctrl);    //确认输入的数字在文本中的位置                     
            //alert(lIndex);return;
       var val = ctrl.value;
        if(lIndex != 0 && kindex == 40)                
        {
            window.event.keyCode = 0;
            return;
        }
        if (kindex==41)
        {
            if (lIndex != val.length)
            {
                window.event.keyCode = 0;
                return;
            }
            if (val.substring(0,1) != "(")
            {
                window.event.keyCode = 0;
                return;
            }
            if (val.indexOf(")") >= 0)
            {
                window.event.keyCode = 0;
                return;
            }
        }
        if (kindex>=48 && kindex<=57 )
        {
            
            if (lIndex > val.indexOf("("))
            {
                // Check ")" Exists 
                
                if (val.indexOf(")") >=0 && lIndex <= val.indexOf(")"))
                {
                } else 
                {
                    if (val.indexOf(")") >= 0) 
                    {
                        
                        window.event.keyCode = 0;
                        return; 
                    }
                }
            } else
            {
            
            }
        }
       
       
       if(lIndex != 0 && keycode == 45)  //负号只能出现在第一位
       {
            window.event.keyCode = 0;
            return;
       }
       if(ctrl.value.length > 0)
       {
            var nextChar = getnextChar(ctrl.value,lIndex,0);
            if(lIndex == 0 && nextChar == "-")
            {
                window.event.keyCode = 0;
                return;
            }
        
       }
       
       //not need ','
   }
  
   function IntegerKeyDown(ctrl)
   {
        if(ctrl.readOnly)
        {
            window.event.returnValue = false;
            return;
         } 
        var keycode = window.event.keyCode;
        
        if(!((keycode >= 48 && keycode <=57) || (keycode >=96 && keycode <=105)|| keycode==110 || keycode==190 || keycode==45 || keycode==8 || keycode==46 || keycode==37 || keycode==39 || keycode==9 || keycode == 40 || keycode == 41 ) ) 
        {
            window.event.returnValue = false;
            return;
         }

   }
   
   // redirect to download manager function
   function sendParentRedirectRequest(link)
   {
        Session(COMMAND_REDIRECTURL) = link;
   }
  
   
