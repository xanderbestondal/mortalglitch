<?php
	$filename = $_POST["filename"];
	$data = $_POST["data"];
	// if($text != "")
	// {
	// 	echo("Succes");
	// 	echo("data contains : " . $data);
	$file = fopen($filename . ".txt", "a");
	fwrite($file, $data);
	fclose($file);
	// }else
	// {
	// 	echo("message delivaary faled");
	// }
?>	