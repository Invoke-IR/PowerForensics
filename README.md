<p align="center">
  <img src="https://github.com/Invoke-IR/PowerForensics/blob/master/Images/powerforensic_square_blue_lowres.png?raw=true" width="300" height="300">
</p>

<h1 align="center">PowerForensics - PowerShell Digital Forensics</h1>

<h5 align="center">Developed by <a href="https://twitter.com/jaredcatkinson">@jaredcatkinson</a></h5>


[![Build status](https://ci.appveyor.com/api/projects/status/l8rmlql34xwyvwsc/branch/master?svg=true)](https://ci.appveyor.com/project/Invoke-IR/powerforensics/branch/master)
[![docs status](https://readthedocs.org/projects/powerforensics/badge/?version=latest)](https://powerforensics.readthedocs.io/en/latest/)
[![waffle ready](https://badge.waffle.io/Invoke-IR/PowerForensics.png?label=ready&title=Ready)](https://waffle.io/Invoke-IR/PowerForensics)
[![waffle in progress](https://badge.waffle.io/Invoke-IR/PowerForensics.png?label=in%20progress&title=In%20Progress)](https://waffle.io/Invoke-IR/PowerForensics)

## Overview
The purpose of PowerForensics is to provide an all inclusive framework for hard drive forensic analysis.
PowerForensics currently supports NTFS and FAT file systems, and work has begun on Extended File System and HFS+ support.

All PowerForensics documentation has been moved to <a href="https://powerforensics.readthedocs.io">Read The Docs</a>.

Detailed instructions for installing PowerForensics can be found <a href="http://www.invoke-ir.com/2016/02/installing-powerforensics.html">here</a>.

## Public API
PowerForensics is built on a C# Class Library (Assembly) that provides a public API for forensic tasks.
The public API provides a modular framework for adding to the capabilities exposed by the PowerForensics module.
All of this module's cmdlets are built on this public API and tasks can easily be expanded upon to create new cmdlets.
API documentation can be found <a href="https://powerforensics.readthedocs.io/en/latest/publicapi/">here</a>.

<p align="center">
  <img src="https://github.com/Invoke-IR/PowerForensics/blob/master/Images/powerforensic_square_blue_lowres.png?raw=true" width="300" height="300">
</p>
