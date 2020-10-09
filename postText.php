<?php
	$usernamer = $_POST["username"];
	$data = $_POST["data"];
	// if($text != "")
	// {
	// 	echo("Succes");
	// 	echo("data contains : " . $data);
	$file = fopen($usernamer . ".txt", "a");
	fwrite($file, $data);
	fclose($file);
	// }else
	// {
	// 	echo("message delivaary faled");
	// }
?>	