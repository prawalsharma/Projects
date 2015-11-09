<?xml version="1.0" encoding="UTF-8"?>

<!--
    Document   : recipes.xsl
    Created on : November 11, 2014, 6:21 PM
    Author     : prawal
    Description:
        Purpose of transformation follows.
-->

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="html"/>

    <!-- TODO customize transformation rules 
         syntax recommendation http://www.w3.org/TR/xslt 
    -->
    <xsl:template match="/">
        <html>
            <head>
                <title>recipes.xsl</title>
            </head>
            <body>
                <h4>
                    <xsl:value-of select="collection/description"/>
                </h4>
                <xsl:for-each select="collection/recipe">
                    <span style="color: gray;">
                        <h3>
                            <xsl:value-of select="title"/>
                        </h3>
                    </span>
                    <span>
                        <xsl:value-of select="date"/>
                    </span>
                    <ul><b>Ingredients:</b> 
                        <xsl:for-each select="ingredient">
                            
                            <li><xsl:value-of select="@name"/> | <xsl:value-of select="@amount"/> | <xsl:value-of select="@unit"/>
                                <ul>
                                    <xsl:for-each select="ingredient">
                                        <li><xsl:value-of select="@name"/> | <xsl:value-of select="@amount"/> | <xsl:value-of select="@unit"/></li>
                                    </xsl:for-each>
                                </ul>
                                <ol>
                                    <xsl:for-each select="preparation/step">
                                        <li><xsl:value-of select="text()"/></li>
                                    </xsl:for-each>
                                </ol>
                            </li>
                            
                        </xsl:for-each>
                    </ul>
                    <ol><b>Preparations:</b>
                        <xsl:for-each select="preparation/step">
                            
                            <li><xsl:value-of select="text()"/></li>
                                      
                        </xsl:for-each>
                    </ol>
                     
                </xsl:for-each>
            </body>
        </html>
    </xsl:template>

</xsl:stylesheet>
