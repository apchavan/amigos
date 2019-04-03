<%@ Page Title="" Language="C#" MasterPageFile="~/AccountMaster/UserAccountMaster.master" AutoEventWireup="true" CodeFile="LetsChat.aspx.cs" Inherits="LetsChat_LetsChat" %>

<asp:Content ID="letsChat_Head" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="LetsChat_StyleSheet.css" />
    <title>Chat with friends 💬🤗😊</title>
</asp:Content>


<asp:Content ID="letsChat_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">

    <!-- Demo template reference :=> https://bootsnipp.com/snippets/1ea0N -->

    <div class="container">
        <div style="text-align: center; top: 0;">
            <asp:Label ID="heading_Label" Text="" runat="server"></asp:Label>
            <hr />
        </div>

        <asp:ScriptManager ID="chatWindow_ScriptManager" runat="server"></asp:ScriptManager>

        <div id="chatWindow" class="messaging" runat="server">
            <div class="inbox_msg">
                <div class="inbox_people">
                    <div class="heading_srch">
                        <div class="text-center" style="font-family: 'Comic Sans MS';">
                            <!-- recent_heading  -->
                            <h5>
                                <asp:Label ID="yourFriends_Label" Text="Your friends" runat="server"></asp:Label>
                            </h5>
                        </div>
                    </div>
                    <hr />

                    <div class="inbox_chat">

                        <div class="chat_list">
                            <div class="chat_people">

                                <asp:UpdatePanel ID="chatWindow_UpdatePanel" runat="server">
                                    <ContentTemplate>
                                        <asp:DataList ID="chatFriends_DataList" runat="server" OnItemCommand="chatFriends_DataList_ItemCommand" OnItemDataBound="chatFriends_DataList_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="chat_img">
                                                    <!--<img src="https://ptetutorials.com/images/user-profile.png" alt="sunil" />-->
                                                    <asp:Image ID="otherUserProfile_Image" ImageUrl='<%#Eval("photo") %>' CssClass="rounded-circle" Width="200" Height="30" oncontextmenu="return false;" runat="server" />
                                                </div>

                                                <div class="chat_ib">
                                                    <asp:LinkButton ID="otherUserName_LinkBtn" CommandName="otherUserName_LinkBtn_CommandName" CssClass="otherUserNameLinkBtn" ToolTip="Click to begin chat..."
                                                        oncontextmenu="return false;"
                                                        Text='<%# String.Format("{0} {1}", Eval("firstname"), Eval("lastname")) %>' runat="server" />
                                                    <asp:Label ID="chatSelection_Label" runat="server" Text=""></asp:Label>
                                                </div>

                                                <br />
                                                <hr />

                                            </ItemTemplate>
                                        </asp:DataList>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                        </div>

                    </div>
                </div>

                <div class="mesgs">

                    <div class="msg_history">
                        <div class="heading_srch">
                            <div class="text-center" style="font-family: 'Comic Sans MS'; font-size: x-large;">
                                <!-- recent_heading  -->
                                <asp:Label ID="chatFriendName_Label" Text="" runat="server"></asp:Label>
                            </div>
                        </div>
                        <hr />

                        <asp:UpdatePanel ID="chatMessages_UpdatePanel" runat="server">
                            <ContentTemplate>


                                <asp:DataList ID="chatMessages_DataList" Width="100%" runat="server" OnItemDataBound="chatMessages_DataList_ItemDataBound">
                                    <ItemTemplate>

                                        <div id="receivedMessageRow_Div" style="margin-bottom: 3.5%;" class="incoming_msg" runat="server">
                                            <div class="incoming_msg_img">
                                                <!--<img src="https://ptetutorials.com/images/user-profile.png" alt="sunil" />-->
                                                <asp:Image ID="otherUserProfile_Image" ImageUrl='<%#Eval("photo") %>' CssClass="rounded-circle" oncontextmenu="return false;" runat="server" />
                                            </div>
                                            <div class="received_msg">
                                                <div class="received_withd_msg">
                                                    <p>
                                                        <asp:Label ID="receivedMessage_Label" Text='<%#Eval("chat_message") %>' runat="server" />
                                                    </p>
                                                    <span class="time_date">
                                                        <asp:Label ID="receivedMessageTime_Label" Text='<%#Eval("chat_date_time") %>' runat="server" />
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                        <div id="sentMessageRow_Div" style="margin-bottom: 3.5%" class="outgoing_msg" runat="server">
                                            <div class="sent_msg">
                                                <p>
                                                    <asp:Label ID="sentMessage_Label" Text='<%#Eval("chat_message") %>' runat="server" />
                                                </p>
                                                <span class="time_date">
                                                    <asp:Label ID="sentMessageTime_Label" Text='<%#Eval("chat_date_time") %>' runat="server" />
                                                </span>
                                            </div>
                                        </div>

                                    </ItemTemplate>
                                </asp:DataList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="chat_Timer" EventName="Tick" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:Timer ID="chat_Timer" runat="server" Interval="500" Enabled="False" OnTick="chat_Timer_Tick" />

                    </div>

                    <div class="type_msg">
                        <div class="input_msg_write">
                            <asp:TextBox ID="sendMessage_TextBox" Style="resize: none;" TextMode="MultiLine" Width="540" CssClass="sendMessageTextBox" placeholder="💬 Type a message ..." runat="server"></asp:TextBox>
                            <asp:Button ID="sendMessage_Btn" OnClick="sendMessage_Btn_Click" CssClass="btn btn-outline-success sendMessageBtn" BorderStyle="Dotted" BorderColor="GradientActiveCaption"
                                Width="100" Text="Send ✈" runat="server" />
                        </div>
                    </div>

                </div>
            </div>

        </div>


    </div>
</asp:Content>

