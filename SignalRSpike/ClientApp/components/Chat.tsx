import * as React from 'react';
import { HubConnection, TransportType } from '@aspnet/signalr-client';
import { RouteComponentProps } from 'react-router';

export interface ChatState {
    messages : string[],
    message : string
}

export class Chat extends React.Component<RouteComponentProps<{}>, ChatState> {
    private hubConnection : HubConnection;

    constructor() {
        super();
        
        this.state = { messages: [], message: '' };

        this.hubConnection = new HubConnection('/signalr', { transport: TransportType.WebSockets });
        this.hubConnection.on('Message', message => {
            this.setState((prevState : ChatState) => {
                prevState.messages.push(message);
                return prevState;
            });
        });
        
        this.hubConnection.start();
    }

    private onMessageChange(evt : any) {
        const value = evt.target.value;
        this.setState(prevState => {
            return {
                message: value,
                messages: prevState.messages
            }
        });
    }

    public render() {
        return <div>
                   <h1>SignalR Chat</h1>
                   {this.renderMessages()}
                   {this.renderInput()}
               </div>;
    }

    private renderMessages() {
        const messages = this.state.messages.map((message, index) => 
            <li key={index}>{message}</li>);

        return <ul>{messages}</ul>;
    }

    private renderInput() {
        return  <div>
                    <input  type="text" 
                            value={this.state.message} 
                            onChange={(evt) => this.onMessageChange(evt)}/>
                    
                    <input  type="button" 
                            value="Send" 
                            onClick={() => this.sendMessage()}/>
                </div>
    }

    private sendMessage() {
        const message = this.state.message;
        this.setState(prevState => {
            return {
                message: '',
                messages: prevState.messages
            }
        });

        this.hubConnection.invoke("Echo", message);
    }
}