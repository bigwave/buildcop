<?xml version="1.0"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns="http://www.w3.org/TR/xhtml1/strict">

  <xsl:template match="/BuildCopReport">
    <html>
      <head>
        <title>
          BuildCop Report
        </title>
        <style type="text/css">
          body { font-family: Calibri, Verdana, Sans-Serif; }
          table { border-collapse: collapse; border: solid #EEEEEE 1px; }
          td, th { text-align: left; border: solid 1px; vertical-align: top; padding-left: 5px; padding-right: 15px; }
          .reportTitle { }
          .reportStats, .reportStats table { font-size: x-small; margin-top: 10px; }
          .reportBody { }
          .reportFooter { font-size: x-small; color: Gray; }
          .reportDetails { font-size: x-small; }
          .reportControls { font-size: x-small; margin-top: 10px; }
          .buildGroupPanel { margin: 10px 0px 10px 0px; padding: 10px; border: solid #EEEEEE 1px; }
          .buildGroupTitle { cursor: hand; border-bottom: dashed #EEEEEE 1px; }
          .buildGroupStats, .buildGroupStats table { font-size: x-small; margin-top: 10px; }
          .buildGroupBody { }
          .buildFilePanel { margin-left: 20px; }
          .buildFileTitle { cursor: hand; border-bottom: dashed #EEEEEE 1px; }
          .buildFileStats, .buildFileStats table { font-size: x-small; margin-bottom: 10px; }
          .buildFileBody { }
          .buildFileLink { color: Black; }
          .buildFileDetails { }
          .buildFileDetails .Exception { color: Red; font-weight: bold; }
          .buildFileDetails .Error .entryLevel { color: Red; }
          .buildFileDetails .Warning .entryLevel { color: Orange; }
          .buildFileDetails .Information .entryLevel { color: Black; }
          .entryLevel { }
          .entryRule { }
          .entryCode { }
          .entryMessage { }
          .entryDetail { font-size: x-small; color: Gray; }
        </style>
        <script type="text/jscript">
          function ToggleVisibility(blockId)
          {
            var block = document.getElementById(blockId);
            if (block.style.display=='none')
            {
              block.style.display='block';
            }
            else
            {
              block.style.display='none';
            }
          }
          function CollapseAll()
          {
            SetAll('none');
            return false;
          }
          function ExpandAll()
          {
            SetAll('block');
            return false;
          }
          function SetAll(newStyle)
          {
            var nodes = document.getElementsByTagName("div"); 
            for (i = 0; i != nodes.length;i++)
            {    
              var block = nodes[i]; 
              if (block != null)
              { 
                if (block.className == 'buildGroupBody' || block.className == 'buildFileBody')
                {
                  block.style.display = newStyle; 
                }
              } 
            }
          }
        </script>
      </head>
      <body>
        <h1 class="reportTitle">
          BuildCop Report
        </h1>
        <div class="reportBody">
          <table class="reportDetails">
            <tr>
              <td>Report Generated</td>
              <td>
                <xsl:value-of select="/BuildCopReport/@generated" />
              </td>
            </tr>
            <tr>
              <td>Minimum Log Level</td>
              <td>
                <xsl:value-of select="/BuildCopReport/@minimumLogLevel" />
              </td>
            </tr>
            <tr>
              <td>BuildCop Engine Version</td>
              <td>
                <xsl:value-of select="/BuildCopReport/@engineVersion" />
              </td>
            </tr>
            <tr>
              <td>Number of Build Groups analyzed</td>
              <td>
                <xsl:value-of select="count(/BuildCopReport/BuildGroup)" />
              </td>
            </tr>
            <tr>
              <td>Number of Build Files analyzed</td>
              <td>
                <xsl:value-of select="count(/BuildCopReport/BuildGroup/BuildFile)" />
              </td>
            </tr>
          </table>
          <div class="reportStats">
            <h2>Summary</h2>
            <table>
              <tr>
                <th></th>
                <th># Build Groups</th>
                <th># Build Files</th>
                <th># Exceptions</th>
                <th># Errors</th>
                <th># Warnings</th>
                <th># Information</th>
              </tr>
              <tr>
                <td>Summary</td>
                <td>
                  <xsl:value-of select="count(BuildGroup)"/>
                </td>
                <td>
                  <xsl:value-of select="count(BuildGroup/BuildFile[count(Entry)>0])"/>
                </td>
                <td>
                  <xsl:value-of select="count(BuildGroup/BuildFile/Entry[@level='Exception'])"/>
                </td>
                <td>
                  <xsl:value-of select="count(BuildGroup/BuildFile/Entry[@level='Error'])"/>
                </td>
                <td>
                  <xsl:value-of select="count(BuildGroup/BuildFile/Entry[@level='Warning'])"/>
                </td>
                <td>
                  <xsl:value-of select="count(BuildGroup/BuildFile/Entry[@level='Information'])"/>
                </td>
              </tr>
            </table>
          </div>
          <div class="reportControls">
            [ <a href="#" onclick="return CollapseAll();">Collapse All</a> ] [ <a href="#" onclick="return ExpandAll();">Expand All</a> ]
          </div>
          <xsl:apply-templates select="BuildGroup"/>
        </div>
        <div class="reportFooter">
          Created by BuildCop - &#169; 2007 by Jelle Druyts - <a href="http://jelle.druyts.net">http://jelle.druyts.net</a>
        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="BuildGroup">
    <xsl:variable name="buildGroupId" select="generate-id()"/>
    <div class="buildGroupPanel">
      <h2 class="buildGroupTitle">
        <xsl:attribute name="onClick">
          javascript:ToggleVisibility('<xsl:value-of select="$buildGroupId"/>');
        </xsl:attribute>
        Build Group: "<xsl:value-of select="@name" />"
      </h2>
      <div class="buildGroupStats">
        <table>
          <tr>
            <th></th>
            <th># Build Files</th>
            <th># Exceptions</th>
            <th># Errors</th>
            <th># Warnings</th>
            <th># Information</th>
          </tr>
          <tr>
            <td>Summary</td>
            <td>
              <xsl:value-of select="count(BuildFile[count(Entry)>0])"/>
            </td>
            <td>
              <xsl:value-of select="count(BuildFile/Entry[@level='Exception'])"/>
            </td>
            <td>
              <xsl:value-of select="count(BuildFile/Entry[@level='Error'])"/>
            </td>
            <td>
              <xsl:value-of select="count(BuildFile/Entry[@level='Warning'])"/>
            </td>
            <td>
              <xsl:value-of select="count(BuildFile/Entry[@level='Information'])"/>
            </td>
          </tr>
        </table>
      </div>
      <div class="buildGroupBody">
        <xsl:attribute name="id">
          <xsl:value-of select="$buildGroupId"/>
        </xsl:attribute>
        <xsl:apply-templates select="BuildFile"/>&#160;
      </div>
    </div>
  </xsl:template>

  <xsl:template match="BuildFile">
    <xsl:if test="count(Entry)>0">
      <xsl:variable name="buildFileId" select="generate-id()"/>
      <div class="buildFilePanel">
        <h3 class="buildFileTitle">
          <xsl:attribute name="onClick">
            javascript:ToggleVisibility('<xsl:value-of select="$buildFileId"/>');
          </xsl:attribute>
          Build File:
          <a class="buildFileLink">
            <xsl:attribute name="href">
              <xsl:value-of select="@path"/>
            </xsl:attribute>
            <xsl:attribute name="title">
              <xsl:value-of select="@path"/>
            </xsl:attribute>
            <xsl:value-of select="@name" />
          </a>
        </h3>
        <div class="buildFileStats">
          <table>
            <tr>
              <th></th>
              <th># Exceptions</th>
              <th># Errors</th>
              <th># Warnings</th>
              <th># Information</th>
            </tr>
            <tr>
              <td>Summary</td>
              <td>
                <xsl:value-of select="count(Entry[@level='Exception'])"/>
              </td>
              <td>
                <xsl:value-of select="count(Entry[@level='Error'])"/>
              </td>
              <td>
                <xsl:value-of select="count(Entry[@level='Warning'])"/>
              </td>
              <td>
                <xsl:value-of select="count(Entry[@level='Information'])"/>
              </td>
            </tr>
          </table>
        </div>
        <div class="buildFileBody">
          <xsl:attribute name="id">
            <xsl:value-of select="$buildFileId"/>
          </xsl:attribute>
          <table class="buildFileDetails">
            <tr>
              <th>Level</th>
              <th>Rule</th>
              <th>Code</th>
              <th>Message</th>
            </tr>
            <xsl:apply-templates select="Entry"/>
          </table>
        </div>
      </div>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Entry">
    <tr>
      <xsl:attribute name="class">
        <xsl:value-of select="@level"/>
      </xsl:attribute>
      <td class="entryLevel">
        <xsl:value-of select="@level"/>
      </td>
      <td class="entryRule">
        <xsl:value-of select="@rule"/>
      </td>
      <td class="entryCode">
        <xsl:value-of select="@code"/>
      </td>
      <td>
        <div class="entryMessage">
          <xsl:apply-templates select="Message"/>
        </div>
        <xsl:choose>
          <xsl:when test="Detail != ''">
            <div class="entryDetail">
              <xsl:apply-templates select="Detail"/>
            </div>
          </xsl:when>
        </xsl:choose>
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="text()">
    <xsl:call-template name="break" />
  </xsl:template>

  <xsl:template name="break">
    <xsl:param name="text" select="."/>
    <xsl:choose>
      <xsl:when test="contains($text, '&#xa;')">
        <xsl:value-of select="substring-before($text, '&#xa;')"/>
        <br/>
        <xsl:call-template name="break">
          <xsl:with-param name="text" select="substring-after($text,'&#xa;')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$text"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>