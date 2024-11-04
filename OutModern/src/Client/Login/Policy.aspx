<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Policy.aspx.cs" Inherits="OutModern.src.Client.Login.Policy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>OutModern | Sign Up</title>

    <!-- import font type css-->
    <link href="<%=Page.ResolveClientUrl("~/lib/fonts/ChakraPetch/ChakraPetch.css") %>" rel="stylesheet" />

    <!-- import fontawesome css-->
    <link href="<%= Page.ResolveClientUrl("~/lib/fontawesome/css/all.min.css") %>" rel="stylesheet" />

    <link href="css/Policy.css" rel="stylesheet" />

    <!-- import tailwind js-->
    <script src="<%= Page.ResolveClientUrl("~/lib/tailwind/tailwind.js") %>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="back">
            <asp:HyperLink ID="hl_back" runat="server" class="backText" NavigateUrl="~/src/Client/Login/SignUp.aspx">< Back</asp:HyperLink>
        </div>
        <div class="policy-wrapper">
            <div class="privacy-content-head">
                <h1>PRIVACY NOTICE</h1>
                <h3><i>Last updated July 30, 2021</i></h3>
                <p>
                    Thank you for choosing to be part of our community at OutModern ("<b>Company</b>", "<b>we</b>",
                    "<b>us</b>", "<b>our</b>"). We are committed to protecting your personal information and your right
                    to privacy. If you have any questions or concerns about this privacy notice, or our practices with
                    regards to your personal information, please contact us at <a
                        href="mailto:ziyanyapzyy@gmail.com">ziyanyapzyy@gmail.com</a>.
                </p>
                <p>
                    When you visit our website <a
                        href="https://outlan7modern.ziyanyap.repl.co">https://outlan7modern.ziyanyap.repl.co</a>
                    (the "<b>Website</b>"), and more generally, use any of our services (the "<b>Services</b>", which
                    include the Website), we appreciate that you are trusting us with your personal information. We take
                    your privacy very seriously. In this privacy notice, we seek to explain to you in the clearest way
                    possible what information we collect, how we use it and what rights you have in relation to it. We
                    hope you take some time to read through it carefully, as it is important. If there are any terms in
                    this privacy notice that you do not agree with, please discontinue use of our Services immediately.
                </p>
                <p>
                    This privacy notice applies to all information collected through our Services (which, as described
                    above, includes our Website), as well as, any related services, sales, marketing or events.
                </p>
                <p>
                    <b>Please read this privacy notice carefully as it will help you understand what we do with the
                        information that we collect.</b>
                </p>
                <h2>TABLE OF CONTENTS</h2>
                <ol>
                    <li><a href="#privacy-content1">WHAT INFORMATION DO WE COLLECT?</a></li>
                    <li><a href="#privacy-content2">HOW DO WE USE YOUR INFORMATION?</a></li>
                    <li><a href="#privacy-content3">WILL YOUR INFORMATION BE SHARED WITH ANYONE?</a></li>
                    <li><a href="#privacy-content4">HOW DO WE HANDLE YOUR SOCIAL LOGINS?</a></li>
                    <li><a href="#privacy-content5">HOW LONG DO WE KEEP YOUR INFORMATION?</a></li>
                    <li><a href="#privacy-content6">HOW DO WE KEEP YOUR INFORMATION SAFE?</a></li>
                    <li><a href="#privacy-content7">WHAT ARE YOUR PRIVACY RIGHTS?</a></li>
                    <li><a href="#privacy-content8">CONTROLS FOR DO-NOT-TRACK FEATURES</a></li>
                    <li><a href="#privacy-content9">DO WE MAKE UPDATES TO THIS NOTICE?</a></li>
                    <li><a href="#privacy-content10">HOW CAN YOU CONTACT US ABOUT THIS NOTICE?</a></li>
                    <li><a href="#privacy-content11">HOW CAN YOU REVIEW, UPDATE OR DELETE THE DATA WE COLLECT FROM
                            YOU?</a></li>
                </ol>
            </div>
            <div class="privacy-content">
                <ol>
                    <!-- 1ST -->
                    <h2 id="privacy-content1">
                        <li>WHAT INFORMATION DO WE COLLECT?</li>
                    </h2>
                    <p>
                        <b>Personal information you disclose to us</b>
                    </p>
                    <p>
                        <i><b>In Short:</b> We collect personal information that you provide to us.</i>
                    </p>
                    <p>
                        We collect personal information that you voluntarily provide to us when you register on the
                        Website, express an interest in obtaining information about us or our products and Services,
                        when you participate in activities on the Website or otherwise when you contact us.
                    </p>
                    <p>
                        The personal information that we collect depends on the context of your interactions with us and
                        the Website, the choices you make and the products and features you use. The personal
                        information we collect may include the following:
                    </p>
                    <p>
                        <b>Personal Information Provided by You.</b> We collect names; phone numbers; email addresses;
                        mailing addresses; usernames; passwords; billing addresses; debit/credit card numbers; contact
                        preferences; delivery addresses; and other similar information.
                    </p>
                    <p>
                        <b>Payment Data.</b> We may collect data necessary to process your payment if you make
                        purchases, such as your payment instrument number (such as a credit card number), and the
                        security code associated with your payment instrument. All payment data is stored by OutModern.
                        You may find their privacy notice link(s) here: <a
                            href="https://outlan7modern.ziyanyap.repl.co">https://outlan7modern.ziyanyap.repl.co</a>.
                    </p>
                    <p>
                        <b>Social Media Login Data.</b> We may provide you with the option to register with us using
                        your existing social media account details, like your Facebook, Twitter or other social media
                        account. If you choose to register in this way, we will collect the information described in the
                        section called <a href="#privacy-content4">"HOW DO WE HANDLE YOUR SOCIAL LOGINS?"</a> below.
                    </p>
                    <p>
                        All personal information that you provide to us must be true, complete and accurate, and you
                        must notify us of any changes to such personal information.
                    </p>
                    <p>
                        <b>Information collected from other sources</b>
                    </p>
                    <p>
                        <i><b>In Short:</b> We may collect limited data from public databases, marketing partners,
                            social media platforms, and other outside sources.</i>
                    </p>
                    <p>
                        In order to enhance our ability to provide relevant marketing, offers and services to you and
                        update our records, we may obtain information about you from other sources, such as public
                        databases, joint marketing partners, affiliate programs, data providers, social media platforms,
                        as well as from other third parties. This information includes mailing addresses, job titles,
                        email addresses, phone numbers, intent data (or user behavior data), Internet Protocol (IP)
                        addresses, social media profiles, social media URLs and custom profiles, for purposes of
                        targeted advertising and event promotion. If you interact with us on a social media platform
                        using your social media account (e.g. Facebook or Twitter), we receive personal information
                        about you such as your name, email address, and gender. Any personal information that we collect
                        from your social media account depends on your social media account's privacy settings.
                    </p>

                    <!-- 2ND -->
                    <h2 id="privacy-content2">
                        <li>HOW DO WE USE YOUR INFORMATION?</li>
                    </h2>
                    <p>
                        <i><b>In Short:</b> We process your information for purposes based on legitimate business
                            interests, the fulfillment of our contract with you, compliance with our legal obligations,
                            and/or your consent.</i>
                    </p>
                    <p>
                        We use personal information collected via our Website for a variety of business purposes
                        described below. We process your personal information for these purposes in reliance on our
                        legitimate business interests, in order to enter into or perform a contract with you, with your
                        consent, and/or for compliance with our legal obligations. We indicate the specific processing
                        grounds we rely on next to each purpose listed below.
                    </p>
                    <p>
                        We use the information we collect or receive:
                    </p>
                    <ul>
                        <p>
                            <li>
                                <b>To facilitate account creation and logon process.</b> If you choose to link your
                                account with us to a third-party account (such as your Google or Facebook account), we
                                use the information you allowed us to collect from those third parties to facilitate
                                account creation and logon process for the performance of the contract. See the section
                                below headed "HOW DO WE HANDLE YOUR SOCIAL LOGINS?" for further information.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>To post testimonials.</b> We post testimonials on our Website that may contain
                                personal information. Prior to posting a testimonial, we will obtain your consent to use
                                your name and the content of the testimonial. If you wish to update, or delete your
                                testimonial, please contact us at <a
                                    href="mailto:ziyanyapzyy@gmail.com">ziyanyapzyy@gmail.com</a> and be sure to include
                                your name, testimonial location, and contact information.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>Request feedback.</b> We may use your information to request feedback and to contact
                                you about your use of our Website.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>To enable user-to-user communications.</b> We may use your information in order to
                                enable user-to-user communications with each user's consent.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>To manage user accounts.</b> We may use your information for the purposes of managing
                                our account and keeping it in working order.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>To send administrative information to you.</b> We may use your personal information
                                to send you product, service and new feature information and/or information about
                                changes to our terms, conditions, and policies.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>To protect our Services.</b> We may use your information as part of our efforts to
                                keep our Website safe and secure (for example, for fraud monitoring and prevention).
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>To enforce our terms, conditions and policies for business purposes, to comply with
                                    legal and regulatory requirements or in connection with our contract.</b>
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>To respond to legal requests and prevent harm.</b> If we receive a subpoena or other
                                legal request, we may need to inspect the data we hold to determine how to respond.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>Fulfill and manage your orders.</b> We may use your information to fulfill and manage
                                your orders, payments, returns, and exchanges made through the Website.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>Administer prize draws and competitions.</b> We may use your information to
                                administer prize draws and competitions when you elect to participate in our
                                competitions.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>To deliver and facilitate delivery of services to the user.</b> We may use your
                                information to provide you with the requested service.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>To respond to user inquiries/offer support to users.</b> We may use your information
                                to respond to your inquiries and solve any potential issues you might have with the use
                                of our Services.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>To send you marketing and promotional communications.</b> We and/or our third-party
                                marketing partners may use the personal information you send to us for our marketing
                                purposes, if this is in accordance with your marketing preferences. For example, when
                                expressing an interest in obtaining information about us or our Website, subscribing to
                                marketing or otherwise contacting us, we will collect personal information from you. You
                                can opt-out of our marketing emails at any time (see the <a
                                    href="#privacy-content7">"WHAT ARE YOUR PRIVACY RIGHTS?"</a> below).
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>Deliver targeted advertising to you.</b> We may use your information to develop and
                                display personalized content and advertising (and work with third parties who do so)
                                tailored to your interests and/or location and to measure its effectiveness.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>For other business purposes.</b> We may use your information for other business
                                purposes, such as data analysis, identifying usage trends, determining the effectiveness
                                of our promotional campaigns and to evaluate and improve our Website, products,
                                marketing and your experience. We may use and store this information in aggregated and
                                anonymized form so that it is not associated with individual end users and does not
                                include personal information. We will not use identifiable personal information without
                                your consent.
                            </li>
                        </p>
                    </ul>

                    <!-- 3RD -->
                    <h2 id="privacy-content3">
                        <li>WILL YOUR INFORMATION BE SHARED WITH ANYONE?</li>
                    </h2>
                    <p>
                        <i><b>In Short:</b> We only share information with your consent, to comply with laws, to provide
                            you with services, to protect your rights, or to fulfill business obligations.</i>
                    </p>
                    <p>
                        We may process or share your data that we hold based on the following legal basis:
                    </p>
                    <ul>
                        <p>
                            <li>
                                <b>Consent:</b> We may process your data if you have given us specific consent to use
                                your personal information for a specific purpose.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>Legitimate Interests:</b> We may process your data when it is reasonably necessary to
                                achieve our legitimate business interests.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>Performance of a Contract:</b> Where we have entered into a contract with you, we may
                                process your personal information to fulfill the terms of our contract.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>Legal Obligations:</b> We may disclose your information where we are legally required
                                to do so in order to comply with applicable law, governmental requests, a judicial
                                proceeding, court order, or legal process, such as in response to a court order or a
                                subpoena (including in response to public authorities to meet national security or law
                                enforcement requirements).
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>Vital Interests:</b> We may disclose your information where we believe it is
                                necessary to investigate, prevent, or take action regarding potential violations of our
                                policies, suspected fraud, situations involving potential threats to the safety of any
                                person and illegal activities, or as evidence in litigation in which we are involved.
                            </li>
                        </p>
                    </ul>
                    <p>
                        More specifically, we may need to process your data or share your personal information in the
                        following situations:
                    </p>
                    <ul>
                        <p>
                            <li>
                                <b>Business Transfers.</b> We may share or transfer your information in connection with,
                                or during negotiations of, any merger, sale of company assets, financing, or acquisition
                                of all or a portion of our business to another company.
                            </li>
                        </p>
                        <p>
                            <li>
                                <b>Business Partners.</b> We may share your information with our business partners to
                                offer you certain products, services or promotions.
                            </li>
                        </p>
                    </ul>

                    <!-- 4TH -->
                    <h2 id="privacy-content4">
                        <li>HOW DO WE HANDLE YOUR SOCIAL LOGINS?</li>
                    </h2>
                    <p>
                        <i><b>In Short:</b> If you choose to register or log in to our services using a social media
                            account, we may have access to certain information about you.</i>
                    </p>
                    <p>
                        Our Website offers you the ability to register and login using your third-party social media
                        account details (like your Facebook or Twitter logins). Where you choose to do this, we will
                        receive certain profile information about you from your social media provider. The profile
                        information we receive may vary depending on the social media provider concerned, but will often
                        include your name, email address, friends list, profile picture as well as other information you
                        choose to make public on such social media platform.
                    </p>
                    <p>
                        We will use the information we receive only for the purposes that are described in this privacy
                        notice or that are otherwise made clear to you on the relevant Website. Please note that we do
                        not control, and are not responsible for, other uses of your personal information by your
                        third-party social media provider. We recommend that you review their privacy notice to
                        understand how they collect, use and share your personal information, and how you can set your
                        privacy preferences on their sites and apps.
                    </p>

                    <!-- 5TH -->
                    <h2 id="privacy-content5">
                        <li>HOW LONG DO WE KEEP YOUR INFORMATION?</li>
                    </h2>
                    <p>
                        <i><b>In Short:</b> We keep your information for as long as necessary to fulfill the purposes
                            outlined in this privacy notice unless otherwise required by law.</i>
                    </p>
                    <p>
                        We will only keep your personal information for as long as it is necessary for the purposes set
                        out in this privacy notice, unless a longer retention period is required or permitted by law
                        (such as tax, accounting or other legal requirements). No purpose in this notice will require us
                        keeping your personal information for longer than one (1) month past the termination of the
                        user's account.
                    </p>
                    <p>
                        When we have no ongoing legitimate business need to process your personal information, we will
                        either delete or anonymize such information, or, if this is not possible (for example, because
                        your personal information has been stored in backup archives), then we will securely store your
                        personal information and isolate it from any further processing until deletion is possible.
                    </p>

                    <!-- 6TH -->
                    <h2 id="privacy-content6">
                        <li>HOW DO WE KEEP YOUR INFORMATION SAFE?</li>
                    </h2>
                    <p>
                        <i><b>In Short:</b> We aim to protect your personal information through a system of
                            organizational and technical security measures.</i>
                    </p>
                    <p>
                        We have implemented appropriate technical and organizational security measures designed to
                        protect the security of any personal information we process. However, despite our safeguards and
                        efforts to secure your information, no electronic transmission over the Internet or information
                        storage technology can be guaranteed to be 100% secure, so we cannot promise or guarantee that
                        hackers, cybercriminals, or other unauthorized third parties will not be able to defeat our
                        security, and improperly collect, access, steal, or modify your information. Although we will do
                        our best to protect your personal information, transmission of personal information to and from
                        our Website is at your own risk. You should only access the Website within a secure environment.
                    </p>

                    <!-- 7TH -->
                    <h2 id="privacy-content7">
                        <li>WHAT ARE YOUR PRIVACY RIGHTS?</li>
                    </h2>
                    <p>
                        <i><b>In Short:</b> You may review, change, or terminate your account at any time. If you are a
                            resident in the EEA or UK and you believe we are unlawfully processing your personal
                            information, you also have the right to complain to your local data protection supervisory
                            authority. You can find their contact details here: <a
                                href="http://ec.europa.eu/justice/data-protection/bodies/authorities/index_en.htm">http://ec.europa.eu/justice/data-protection/bodies/authorities/index_en.htm</a>.</i>
                    </p>
                    <p>
                        If you are a resident in Switzerland, the contact details for the data protection authorities
                        are available here: <a
                            href="https://www.edoeb.admin.ch/edoeb/en/home.html">https://www.edoeb.admin.ch/edoeb/en/home.html</a>.
                    </p>
                    <p>
                        If you have questions or comments about your privacy rights, you may email us at <a
                            href="mailto:ziyanyapzyy@gmail.com">ziyanyapzyy@gmail.com</a>.
                    </p>
                    <p>
                        <b>Account Information</b>
                    </p>
                    <p>
                        If you would at any time like to review or change the information in your account or terminate
                        your account, you can:
                    </p>
                    <ul>
                        <p>
                            <li>Log in to your account settings and update your user account.</li>
                        </p>
                    </ul>
                    <p>
                        Upon your request to terminate your account, we will deactivate or delete your account and
                        information from our active databases. However, we may retain some information in our files to
                        prevent fraud, troubleshoot problems, assist with any investigations, enforce our Terms of Use
                        and/or comply with applicable legal requirements.
                    </p>
                    <p>
                        <b>Opting out of email marketing:</b> You can unsubscribe from our marketing email list at any
                        time by clicking on the unsubscribe link in the emails that we send or by contacting us using
                        the details provided below. You will then be removed from the marketing email list — however, we
                        may still communicate with you, for example to send you service-related emails that are
                        necessary for the administration and use of your account, to respond to service requests, or for
                        other non-marketing purposes. To otherwise opt-out, you may:
                    </p>
                    <ul>
                        <p>
                            <li>Access your account settings and update your preferences.</li>
                        </p>
                    </ul>

                    <!-- 8TH -->
                    <h2 id="privacy-content8">
                        <li>CONTROLS FOR DO-NOT-TRACK FEATURES</li>
                    </h2>
                    <p>
                        Most web browsers and some mobile operating systems and mobile applications include a
                        Do-Not-Track ("DNT") feature or setting you can activate to signal your privacy preference not
                        to have data about your online browsing activities monitored and collected. At this stage no
                        uniform technology standard for recognizing and implementing DNT signals has been finalized. As
                        such, we do not currently respond to DNT browser signals or any other mechanism that
                        automatically communicates your choice not to be tracked online. If a standard for online
                        tracking is adopted that we must follow in the future, we will inform you about that practice in
                        a revised version of this privacy notice.
                    </p>

                    <!-- 9TH -->
                    <h2 id="privacy-content9">
                        <li>DO WE MAKE UPDATES TO THIS NOTICE?</li>
                    </h2>
                    <p>
                        <i><b>In Short:</b> Yes, we will update this notice as necessary to stay compliant with relevant
                            laws.</i>
                    </p>
                    <p>
                        We may update this privacy notice from time to time. The updated version will be indicated by an
                        updated "Revised" date and the updated version will be effective as soon as it is accessible. If
                        we make material changes to this privacy notice, we may notify you either by prominently posting
                        a notice of such changes or by directly sending you a notification. We encourage you to review
                        this privacy notice frequently to be informed of how we are protecting your information.
                    </p>

                    <!-- 10TH -->
                    <h2 id="privacy-content10">
                        <li>HOW CAN YOU CONTACT US ABOUT THIS NOTICE?</li>
                    </h2>
                    <p>
                        If you have questions or comments about this notice, you may email us at <a
                            href="mailto:ziyanyapzyy@gmail.com">ziyanyapzyy@gmail.com</a> or by post to:
                    </p>
                    <p>
                        OutModern<br>
                        30 3 01, Jln Radin Anum<br>
                        Taman Seri Petaling<br>
                        57000 Kuala Lumpur<br>
                        Malaysia
                    </p>

                    <!-- 11TH -->
                    <h2 id="privacy-content11">
                        <li>HOW CAN YOU REVIEW, UPDATE OR DELETE THE DATA WE COLLECT FROM YOU?</li>
                    </h2>
                    <p>
                        Based on the applicable laws of your country, you may have the right to request access to the
                        personal information we collect from you, change that information, or delete it in some
                        circumstances. To request to review, update, or delete your personal information, please visit:
                        <a href="https://outlan7modern.ziyanyap.repl.co">https://outlan7modern.ziyanyap.repl.co</a>.
                    </p>
                </ol>
            </div>
        </div>

    </form>
</body>
</html>
