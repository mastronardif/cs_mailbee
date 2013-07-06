<?xml version="1.0" encoding="ISO-8859-1"?>

<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/*/*">
  <html>
  <body>
  <br/>
  <table border="1">
<tr>
<th bgcolor="red" colspan="100%"><xsl:value-of select="local-name()"/></th>
</tr>

    <tr bgcolor="#9acd32">
      <th>Left</th>
      <th>Right</th>
    </tr>
    <xsl:for-each select="*">
    <tr>
      <td>
<xsl:value-of select="local-name()"/>  
( 
<span style="color:green">  
<xsl:value-of select="."/>
</span>
 )   
</td>

      <td>
<xsl:value-of select="."/>
</td>
    </tr>
    </xsl:for-each>
  </table>
  </body>
  </html>
</xsl:template>

</xsl:stylesheet> 