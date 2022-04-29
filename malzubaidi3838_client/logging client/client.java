/*
  FILE : client.java
* PROJECT :  Assignment 3#
* PROGRAMMER : Mahmood Al-Zubaidi
* FIRST VERSION : 25 Feb/2022
* DESCRIPTION : The purpose of this function is to demosntarte the use of a client who manually and autumaticlly
  sends logg messages to the service, as well as a set of autumated messages at once. 
*/

import java.net.*;
import java.util.Properties;
import java.io.*;
import java.util.Scanner;



public class client {
    public static void main(String[] args) {
        String SystemName = retHostName();
        Properties props = retProp();
        String msgToSend = null;
        String hostname = props.getProperty("ipAdrress");
        String port = props.getProperty("port");
        int portInt = Integer.parseInt(port);



        Scanner myObj = new Scanner(System.in);
        System.out.println("Write 1 to start sending log messages to the service manually, Write 2 to send a number of autumated messages to the service, write 3 to autumaticlly send all types of messages and formats the service supports");
        String userInput = myObj.nextLine();
        if(userInput.equals("1")){
            while(true){
                msgToSend = manualChoiceHandler();
                connectionHandeler(hostname, portInt, SystemName, msgToSend);
            }
            
        }
        else if(userInput.equals("2")){
            while(true){
                System.out.println("Insert the type of text that you want to send to the client autumaticly");
                System.out.println("Choices...");
                System.out.println("choose 1 to send an Info message");
                System.out.println("choose 2 to send a Warning message");
                System.out.println("choose 3 to send a Fatal message");
                System.out.println("choose 4 to send a Debug message");
                System.out.println("choose 5 to send an error message");

                String choice = myObj.nextLine();
                if(choice.equals("1")){
                    msgToSend = props.getProperty("info");
                    autumaticSender(hostname, portInt, SystemName, msgToSend);
                }
                
                else if(choice.equals("2")){
                    msgToSend = props.getProperty("warn");
                    autumaticSender(hostname, portInt, SystemName, msgToSend);
                }
                
                else if(choice.equals("3")){
                    msgToSend = props.getProperty("fatal");
                    autumaticSender(hostname, portInt, SystemName, msgToSend);
                }
                
                else if(choice.equals("4")){
                    msgToSend = props.getProperty("debug");
                    autumaticSender(hostname, portInt, SystemName, msgToSend);
                }
                
                else if(choice.equals("5")){
                    msgToSend = props.getProperty("error");
                    autumaticSender(hostname, portInt, SystemName, msgToSend);
                }
                
            }
            

        }
        else if(userInput.equals("3")){
            msgToSend = props.getProperty("info");
            connectionHandeler(hostname, portInt, SystemName, msgToSend);
            msgToSend = props.getProperty("warn");
            connectionHandeler(hostname, portInt, SystemName, msgToSend);
            msgToSend = props.getProperty("fatal");
            connectionHandeler(hostname, portInt, SystemName, msgToSend);
            msgToSend = props.getProperty("debug");
            connectionHandeler(hostname, portInt, SystemName, msgToSend);
            msgToSend = props.getProperty("error");
            connectionHandeler(hostname, portInt, SystemName, msgToSend);
        }
    }

    
    
    /** Function: connectionHandeler
    * Description: It opens a socket client, connects to the server and sends messages to the server.
    * Parameters: String hostname, int portInt, String SystemName, String msgToSend
    * Returns: none.
    */
    public static void connectionHandeler(String hostname, int portInt, String SystemName, String msgToSend){
        try (Socket socket = new Socket(hostname, portInt)) {
            
            OutputStream output = socket.getOutputStream();
            PrintWriter writer = new PrintWriter(output, true);
            writer.println(SystemName+":"+msgToSend);
            socket.close();
 
        } catch (UnknownHostException ex) {
 
            System.out.println("Server not found: " + ex.getMessage());
 
        } catch (IOException ex) {
 
            System.out.println("I/O error: " + ex.getMessage());
        }
    }


    
    
    /** Function: retProp()
    * Description: It retrieves the properties of the config file.
    * Parameters: none.
    * Returns: none.
    */
    public static Properties retProp(){
        try(FileReader reader =  new FileReader("config"))
         {
            Properties properties = new Properties();
            properties.load(reader);
            return properties;
           
        }
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }


    
    /** Function: retHostName
    * Description: It returns the name of the machine's owner.
    * Parameters: none.
    * Returns: name, a string that contains the machine's owner.
    */
    public static String retHostName(){
        String name;
        try {
            name = InetAddress.getLocalHost().getHostName();
        }
        catch (Exception E) {
            System.err.println(E.getMessage());
            name = null;
        }
        return name;
    }
    


    
    /** Function: manualChoiceHandler()
    * Description: It promps the user to get what type of message that they want to send to the service. And retrieves that message value from the config file.
    * Parameters: none.
    * Returns: msg, which contains the message that the user wants to send.
    */
    public static String manualChoiceHandler(){
        String msg = null;
        Scanner myObj = new Scanner(System.in);
        Properties props = retProp();
        System.out.println("What type of log message do you want to send (knowing that you can manually configure it's content in the config file");
        System.out.println("Choices...");
        System.out.println("choose 1 to send an Info message");
        System.out.println("choose 2 to send a Warning message");
        System.out.println("choose 3 to send a Fatal message");
        System.out.println("choose 4 to send a Debug message");
        System.out.println("choose 5 to send an error message");        
        String choice = myObj.nextLine();
        if(choice.equals("1")){
            msg = props.getProperty("info");
        }
        
        else if(choice.equals("2")){
            msg = props.getProperty("warn");
        }
        
        else if(choice.equals("3")){
            msg = props.getProperty("fatal");
        }
        
        else if(choice.equals("4")){
            msg = props.getProperty("debug");
        }
        
        else if(choice.equals("5")){
            msg = props.getProperty("error");
        }
        return msg;
    }



    public static String autumaticChoiceHandler(){
        String msg = null;
        Scanner myObj = new Scanner(System.in);
        Properties props = retProp();
        System.out.println("Insert the autumated text that you want to send to the client");
        String choice = myObj.nextLine();
        if(choice.equals("1")){
            String errMsg = props.getProperty("error");
            msg = errMsg;
        }
        System.out.println("How many times do you want to send that message?");
        String times = myObj.nextLine();
        return msg;
    }



    public static void autumaticSender(String hostname, int portInt, String SystemName, String msgToSend){
        Scanner myObj = new Scanner(System.in);
        System.out.println("How many times do you want to send that message?");
            String times = myObj.nextLine();
            int i = 0;
            while(i < Integer.parseInt(times)){
                connectionHandeler(hostname, portInt, SystemName, msgToSend);
                i++;
                try {
                    Thread.sleep(1);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
    }

}