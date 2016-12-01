<?xml version="1.0" encoding="iso-8859-1"?>
<!--
The MIT License (MIT)

Copyright (c) 2015 Jaime Olivares

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

# xmldoc2md.xsl
By Jaime Olivares
URL: http://github.com/jaime-olivares/xmldoc2md
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" omit-xml-declaration="yes" indent="no" />

  <xsl:template match="/">
      <xsl:apply-templates select="//assembly"/>
  </xsl:template>
    
  <!-- Assembly template -->
  <xsl:template match="assembly">
    <xsl:text># </xsl:text>
    <xsl:value-of select="name"/>
    <xsl:text>&#10;</xsl:text>
    <xsl:apply-templates select="//member[contains(@name,'T:')]"/>
  </xsl:template>

  <!-- Type template -->
  <xsl:template match="//member[contains(@name,'T:')]">
    
    <xsl:variable name="FullMemberName" select="substring-after(@name, ':')"/>
    <xsl:variable name="MemberName">
      <xsl:choose>
        <xsl:when test="contains(@name, '.')">
          <xsl:value-of select="substring-after(@name, '.')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring-after(@name, ':')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    
    <xsl:text>&#10;&#10;## </xsl:text>
    <xsl:value-of select="$MemberName"/>

    <xsl:apply-templates />
    
    <!-- Fields -->
    <xsl:if test="//member[contains(@name,concat('F:',$FullMemberName))]">
      <xsl:text>&#10;### Fields</xsl:text>
      <xsl:text>&#10;</xsl:text>

      <xsl:for-each select="//member[contains(@name,concat('F:',$FullMemberName))]">
        <xsl:text>&#10;#### </xsl:text>
        <xsl:value-of select="substring-after(@name, concat('F:',$FullMemberName,'.'))"/>
        <xsl:text>&#10;</xsl:text>
        <xsl:value-of select="normalize-space()" />
      </xsl:for-each>
    </xsl:if>

    <!-- Properties -->
    <xsl:if test="//member[contains(@name,concat('P:',$FullMemberName))]">
      <xsl:text>&#10;### Properties</xsl:text>
      <xsl:text>&#10;</xsl:text>

      <xsl:for-each select="//member[contains(@name,concat('P:',$FullMemberName))]">
        <xsl:text>&#10;#### </xsl:text>
        <xsl:value-of select="substring-after(@name, concat('P:',$FullMemberName,'.'))"/>
        <xsl:text>&#10;</xsl:text>
        <xsl:value-of select="normalize-space()" />
      </xsl:for-each>
    </xsl:if>

    <!-- Methods -->
    <xsl:if test="//member[contains(@name,concat('M:',$FullMemberName))]">
      <xsl:text>&#10;### Methods</xsl:text>
      <xsl:text>&#10;</xsl:text>

      <xsl:for-each select="//member[contains(@name,concat('M:',$FullMemberName))]">

        <!-- If this is a constructor, display the type name (instead of "#ctor"), or display the method name -->
        <xsl:choose>
          <xsl:when test="contains(@name, '#ctor')">
            <xsl:text>&#10;&#10;#### Constructor</xsl:text>
            <!-- xsl:value-of select="$MemberName"/ -->
            <!-- xsl:value-of select="substring-after(@name, '#ctor')"/-->
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>&#10;&#10;#### </xsl:text>
            <xsl:value-of select="substring-after(@name, concat('M:',$FullMemberName,'.'))"/>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:if test="count(remarks)!=0">
          <xsl:apply-templates select="remarks" />
        </xsl:if>

        <xsl:if test="count(summary)!=0">
          <xsl:apply-templates select="summary" />
        </xsl:if>

        <xsl:if test="count(param)!=0">
          <xsl:text>&#10;&gt; ##### Parameters</xsl:text>
          <xsl:apply-templates select="param"/>
        </xsl:if>

        <xsl:if test="count(returns)!=0">
          <xsl:text>&#10;&gt; ##### Return value</xsl:text>
          <xsl:apply-templates select="returns"/>
        </xsl:if>

        <xsl:if test="count(exception)!=0">
          <xsl:text>&#10;&gt; ##### Exceptions</xsl:text>
          <xsl:apply-templates select="exception"/>
        </xsl:if>

        <xsl:if test="count(example)!=0">
          <xsl:text>&#10;&gt; ##### Example</xsl:text>
          <xsl:text>&#10;&gt; </xsl:text><xsl:apply-templates select="example" />
        </xsl:if>

      </xsl:for-each>
    </xsl:if>
  </xsl:template>

  <xsl:template match="summary">
    <xsl:text>&#10;</xsl:text>
    <xsl:value-of select="normalize-space()" />
  </xsl:template>

  <xsl:template match="remarks">
    <xsl:text>&#10;</xsl:text>
    <xsl:value-of select="normalize-space()" />
  </xsl:template>

  <xsl:template match="c">
    <xsl:text>`</xsl:text>
    <xsl:value-of select="normalize-space()" />
    <xsl:text>`</xsl:text>
  </xsl:template>

  <xsl:template match="code">
    <xsl:text>&#10;```&#10;</xsl:text>
    <xsl:value-of select="text()" />
    <xsl:text>```</xsl:text>
  </xsl:template>

  <xsl:template match="exception">
    <xsl:text>&#10;&gt; **</xsl:text><xsl:value-of select="substring-after(@cref,'T:')"/>:** <xsl:value-of select="normalize-space()" /><xsl:text>&#10;</xsl:text>
  </xsl:template>

  <xsl:template match="include">
    [External file]({@file})
  </xsl:template>

  <xsl:template match="para">
    <xsl:value-of select="normalize-space()" />
  </xsl:template>

  <xsl:template match="param">
    <xsl:text>&#10;&gt; **</xsl:text><xsl:value-of select="@name"/>:** <xsl:value-of select="normalize-space()" /><xsl:text>&#10;</xsl:text>
  </xsl:template>

  <xsl:template match="paramref">
    <xsl:text>*</xsl:text>
    <xsl:value-of select="@name" />
    <xsl:text>*</xsl:text>
  </xsl:template>

  <xsl:template match="permission">
    <xsl:text>&#10;**Permission:** *</xsl:text><xsl:value-of select="@cref" />* &#10;<xsl:value-of select="normalize-space()" />
  </xsl:template>

  <xsl:template match="returns">
    <xsl:text>&#10;&gt; </xsl:text>
    <xsl:value-of select="normalize-space()" />
  </xsl:template>

  <xsl:template match="see">
    <xsl:text>&#10;&gt; *See: </xsl:text><xsl:value-of select="@cref" />*
  </xsl:template>

  <xsl:template match="seealso">
    <xsl:text>&#10;&gt; *See also: </xsl:text>
    <xsl:value-of select="@cref" />
  </xsl:template>

</xsl:stylesheet>
