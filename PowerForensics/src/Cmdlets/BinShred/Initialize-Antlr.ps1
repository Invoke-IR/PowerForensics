$env:CLASSPATH = "$psScriptRoot\antlr-4.5.1-complete.jar;" + $env:CLASSPATH
function antlr { java org.antlr.v4.Tool $args }
function grun  { java org.antlr.v4.runtime.misc.TestRig $args }
