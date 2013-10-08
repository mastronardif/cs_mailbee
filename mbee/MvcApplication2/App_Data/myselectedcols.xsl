<?xml version="1.0"?>
<xsl:stylesheet version="1.0"
      xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >

<xsl:template match="/">
<HTML>
<BODY>
    <xsl:apply-templates/>
</BODY>
</HTML>
</xsl:template>

<xsl:template match="/*">
<TABLE BORDER="1">
<TR>
        <xsl:for-each select="*[position() = 1]/*">
          <xsl:choose>
            <xsl:when test="name() ='text' or name() ='user'">
              <!-- Do something -->
              <TD>
                <span style="color:red;">
                  <xsl:value-of select="local-name()"/>
                </span>
              </TD>
            </xsl:when>

            <xsl:otherwise>
              <!-- Default case -->
            <TD>
              <span style="color:green;">
              <xsl:value-of select="local-name()"/>
              </span>
            </TD>
              
            </xsl:otherwise>
            
          </xsl:choose>
          </xsl:for-each>
</TR>
      <xsl:apply-templates/>
</TABLE>
</xsl:template>

<xsl:template match="/*/*">
<TR>
    <xsl:apply-templates/>
</TR>
</xsl:template>

<xsl:template match="/*/*/*">
<TD>
  <xsl:choose>
    <xsl:when test="name() ='text'">
        <xsl:value-of select="."/>
    </xsl:when>

    <xsl:when test="name() ='user'">
      <span style="color:blue;">
      <xsl:value-of select="."/>
      <!--<xsl:value-of select="./profile_image_url"/>-->
      <img  src="{./profile_image_url}" />
      </span>

      <span style="color:black;">
        <br/>
        <xsl:value-of select="./name"/>
        <br/>
      
        <a href="{./url}">
          <xsl:value-of select="./url"></xsl:value-of>
        </a>
        <br/>
        <a href="{./entities/urls/url/expanded_url}">
          <xsl:value-of select="./entities/urls/display_url"></xsl:value-of>
        </a>
      </span>
      
    </xsl:when>
    
    
    <xsl:otherwise>
      <span style="color:pink;">
        <xsl:value-of select="."/>
      </span>
    </xsl:otherwise>
    
  </xsl:choose>
</TD>
</xsl:template>

</xsl:stylesheet>
